using ConvertTextInAudio.Interfaces;
using System.Diagnostics;

namespace ConvertTextInAudio;
public class Worker(
    ILogger<Worker> logger,
    IGenerateAudioService service) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var stopwatch = new Stopwatch();

        try
        {
            await service.GenerateEpubAudioAsync(stoppingToken);
            logger.LogInformation("Iniciando geração de áudio...");
            stopwatch.Start();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao gerar áudio");
        }
    }
}