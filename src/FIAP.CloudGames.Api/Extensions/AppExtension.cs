using FIAP.CloudGames.Api.Middlewares;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace FIAP.CloudGames.Api.Extensions;

public static class AppExtension
{
    public static void UseProjectConfiguration(this WebApplication app)
    {
        app.UseCustomSwagger();
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.UseMiddleware<ExceptionHandlingMiddleware>();
        app.MapControllers();
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
}