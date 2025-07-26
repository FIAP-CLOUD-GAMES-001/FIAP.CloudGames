using FIAP.CloudGames.Api.Filters;
using FIAP.CloudGames.Api.Logging;
using FIAP.CloudGames.Domain.Interfaces.Auth;
using FIAP.CloudGames.Domain.Interfaces.Repositories;
using FIAP.CloudGames.Domain.Interfaces.Services;
using FIAP.CloudGames.Domain.Models;
using FIAP.CloudGames.infrastructure.Data;
using FIAP.CloudGames.infrastructure.Repositories;
using FIAP.CloudGames.Service.Auth;
using FIAP.CloudGames.Service.Game;
using FIAP.CloudGames.Service.User;
using FIAP.CloudGames.Service.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using Serilog;
using Serilog.Events;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace FIAP.CloudGames.Api.Extensions;

public static class BuilderExtension
{
    public static void AddProjectServices(this WebApplicationBuilder builder)
    {
        builder.UseJsonFileConfiguration();
        builder.ConfigureDbContext();
        builder.ConfigureJwt();
        builder.ConfigureLogMongo();
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.ConfigureSwagger();
        builder.ConfigureDependencyInjectionRepository();
        builder.ConfigureDependencyInjectionService();
        builder.ConfigureHealthChecks();
        builder.ConfigureValidators();
    }

    private static void ConfigureHealthChecks(this WebApplicationBuilder builder)
    {
        builder.Services.AddHealthChecks()
            .AddSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")!, name: "sqlserver", timeout: TimeSpan.FromSeconds(5));
    }
    private static void ConfigureDependencyInjectionService(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<ITokenService, TokenService>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IGameService, GameService>();
        builder.Services.AddScoped<IOwnedGameService, OwnedGameService>();
        builder.Services.AddScoped<IPromotionService, PromotionService>();

        builder.Services.AddScoped<OwnedGameAccessFilter>();
    }
    private static void ConfigureDependencyInjectionRepository(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IGameRepository, GameRepository>();
        builder.Services.AddScoped<IOwnedGameRepository, OwnedGameRepository>();
        builder.Services.AddScoped<IPromotionRepository, PromotionRepository>();
    }
    private static void ConfigureJwt(this WebApplicationBuilder builder)
    {
        var configuration = builder.Configuration.GetSection("Jwt");

        builder.Services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Issuer"],
                    ValidAudience = configuration["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Key"]!))
                };

                o.Events = new JwtBearerEvents
                {
                    OnForbidden = context =>
                    {
                        context.Response.ContentType = "application/json";
                        context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                        var apiResponse = ApiResponse<string>.Fail("Authorization Denied", ["You do not have the necessary permissions to access this resource."]);
                        var jsonResponse = JsonSerializer.Serialize(apiResponse);

                        Log.Warning("Forbidden access attempt: {Path}", context.Request.Path);
                        return context.Response.WriteAsync(jsonResponse);
                    },
                    OnChallenge = context =>
                    {
                        context.HandleResponse();
                        context.Response.ContentType = "application/json";
                        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        var apiResponse = ApiResponse<string>.Fail("Authentication Failed", ["Authentication required or invalid credentials."]);
                        var jsonResponse = JsonSerializer.Serialize(apiResponse);

                        Log.Warning("Unauthorized access attempt: {Path}", context.Request.Path);
                        return context.Response.WriteAsync(jsonResponse);
                    }
                };
            });

        builder.Services.AddAuthorization();
        builder.Services.AddScoped<TokenService>();
    }
    private static void ConfigureLogMongo(this WebApplicationBuilder builder)
    {
        var mongoConnection = builder.Configuration.GetConnectionString("MongoDB") ?? string.Empty;

        builder.Services.AddHttpContextAccessor();
        HttpContextAccessorWrapper.Accessor = builder.Services.BuildServiceProvider()
            .GetRequiredService<IHttpContextAccessor>();

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .Enrich.With<UserClaimsEnricher>()
            .WriteTo.Console()
            .WriteTo.MongoDB(mongoConnection, collectionName: "logs")
            .CreateLogger();

        builder.Host.UseSerilog();

        try
        {
            var settings = MongoClientSettings.FromConnectionString(mongoConnection);
            settings.ConnectTimeout = TimeSpan.FromSeconds(5);

            var client = new MongoClient(settings).ListDatabaseNames();
            Log.Information("MongoDB connection successful.");
        }
        catch (Exception ex)
        {
            Log.Error($"MongoDB connection failed: {ex.Message}");
        }
    }
    private static void ConfigureDbContext(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<DataContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
    }
    private static void ConfigureSwagger(this WebApplicationBuilder builder)
    {
        builder.Services.AddSwaggerGen(options =>
        {
            options.SchemaFilter<EnumSchemaFilter>();
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "FiapCloudGamesApi",
                Version = "v1",
                Description = "API Web ASP.NET Core",
            });

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        In = ParameterLocation.Header,
                        Scheme = "bearer"
                    },
                    Array.Empty<string>()
                }
            });

            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });
    }
    private static void ConfigureValidators(this WebApplicationBuilder builder)
    {
        builder.Services.AddValidatorsFromAssemblyContaining<Program>();
        builder.Services.AddValidatorsFromAssemblyContaining<RegisterUserRequestValidator>();
    }
    private static void UseJsonFileConfiguration(this WebApplicationBuilder builder)
    {
        var keysDirectoryPath = Path.Combine(AppContext.BaseDirectory, "dataprotection-keys");
        var keysDirectory = new DirectoryInfo(keysDirectoryPath);

        if (!keysDirectory.Exists)
            keysDirectory.Create();

        builder.Services.AddDataProtection()
            .PersistKeysToFileSystem(keysDirectory)
            .SetApplicationName("FIAP.CloudGames");

        builder.Configuration
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
            .AddJsonFile("appsettings.Secrets.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();
    }
}