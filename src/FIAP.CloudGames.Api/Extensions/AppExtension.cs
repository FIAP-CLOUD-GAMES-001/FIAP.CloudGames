using FIAP.CloudGames.Api.Middlewares;
using FIAP.CloudGames.Domain.Entities;
using FIAP.CloudGames.Domain.Enums;
using FIAP.CloudGames.infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace FIAP.CloudGames.Api.Extensions;

public static class AppExtension
{
    public static void UseProjectConfiguration(this WebApplication app)
    {
        app.UseCustomSwagger();
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseMiddleware<ExceptionHandlingMiddleware>();
        app.MapControllers();
        app.GenerateMigrations();
        app.MapHealthChecks("/health");
    }

    private static void UseCustomSwagger(this WebApplication app)
    {
        if (!app.Environment.IsDevelopment())
            return;

        app.UseSwagger();

        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "FiapCloudGamesApi API v1");
            c.SupportedSubmitMethods([
                SubmitMethod.Get,
                SubmitMethod.Post,
                SubmitMethod.Put,
                SubmitMethod.Delete,
                SubmitMethod.Patch
            ]);
        });
    }
    private static void GenerateMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
        dbContext.Database.Migrate();

        var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();

        var email = config["SeedAdmin:Email"] ?? Environment.GetEnvironmentVariable("SEED_ADMIN_EMAIL");
        var password = config["SeedAdmin:Password"] ?? Environment.GetEnvironmentVariable("SEED_ADMIN_PASSWORD");

        if (!string.IsNullOrEmpty(email) &&
            !string.IsNullOrEmpty(password) &&
            !dbContext.Users.Any(u => u.Email == email))
        {
            var admin = new UserEntity(
                name: "Administrator",
                email: email,
                plainPassword: password,
                role: ERole.Admin
            );

            dbContext.Users.Add(admin);
            dbContext.SaveChanges();
        }
    }
}