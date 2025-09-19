using ConvertTextInAudio.Interfaces;
using System.Net.Http.Json;

namespace ConvertTextInAudio.Integration;
public class GenerateAudioService(IHttpClientFactory httpClientFactory) : IGenerateAudioService
{
    public async Task<byte[]> GenerateAudioAsync(string text, CancellationToken cancellationToken)
    {
        var client = httpClientFactory.CreateClient("kokoroApi");
        var payload = new
        {
            model = "model",
            voice = "pm_santa",
            input = text,
            response_format = "mp3",
            speed = 0.8
        };
        using var response = await client.PostAsJsonAsync("/api/v1/audio/speech", payload, cancellationToken);
        return await response.Content.ReadAsByteArrayAsync(cancellationToken);
    }
}