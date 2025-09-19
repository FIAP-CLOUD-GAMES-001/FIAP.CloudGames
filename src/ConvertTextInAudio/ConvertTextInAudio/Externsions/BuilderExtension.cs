using ConvertTextInAudio.Integration;
using ConvertTextInAudio.Interfaces;
using System.Net.Http.Headers;

namespace ConvertTextInAudio.Externsions;
public static class BuilderExtension
{
    public static void AddServices(this IServiceCollection service)
    {
        service.AddTransient<IGenerateAudioService, GenerateAudioService>();

        service.AddHttpClient("kokoroApi", client =>
        {
            client.Timeout = TimeSpan.FromMinutes(30);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "your-api-key");
            client.BaseAddress = new Uri("http://localhost:3000/");
        });
    }
}