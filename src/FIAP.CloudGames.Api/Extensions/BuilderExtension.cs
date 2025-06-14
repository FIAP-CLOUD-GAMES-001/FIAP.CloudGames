using FIAP.CloudGames.infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace FIAP.CloudGames.Api.Extensions;

public static class BuilderExtension
{
    public static WebApplicationBuilder AddProjectServices(this WebApplicationBuilder builder)
    {
        ConfigureDbContext(builder);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();

        ConfigureSwagger(builder);

        return builder;
    }

    private static void ConfigureDbContext(WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<DataContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
    }

    private static void ConfigureSwagger(WebApplicationBuilder builder)
    {
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("FiapCloudGamesApi v1", new OpenApiInfo
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
}