using FIAP.CloudGames.Domain.Interfaces.Repositories;
using FIAP.CloudGames.Domain.Interfaces.Services;
using FIAP.CloudGames.infrastructure.Data;
using FIAP.CloudGames.infrastructure.Repositories;
using FIAP.CloudGames.Service.Auth;
using FIAP.CloudGames.Service.User;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace FIAP.CloudGames.Api.Extensions;

public static class BuilderExtension
{
    public static void AddProjectServices(this WebApplicationBuilder builder)
    {
        builder.UseJsonFileConfiguration();
        builder.ConfigureDbContext();
        builder.ConfigureJwt();
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
        builder.Services.AddScoped<IUserService, UserService>();
    }
    private static void ConfigureDependencyInjectionRepository(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IUserRepository, UserRepository>();
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
            });

        builder.Services.AddAuthorization();
        builder.Services.AddScoped<TokenService>();
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
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "FiapCloudGamesApi",
                Version = "v1",
                Description = "API Web ASP.NET Core"
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
        });
    }

    private static void ConfigureValidators(this WebApplicationBuilder builder)
    {
        builder.Services.AddValidatorsFromAssemblyContaining<Program>();
    }
    
    public static void UseJsonFileConfiguration(this WebApplicationBuilder builder)
    {
        builder.Configuration
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
            .AddJsonFile("appsettings.Secrets.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();
    }
}