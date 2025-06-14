namespace FIAP.CloudGames.Api.Extensions;

public static class AppExtension
{
    public static WebApplication UseProjectConfiguration(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/FiapCloudGamesApi v1/swagger.json", "FiapCloudGamesApi v1");
            });
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        return app;
    }
}