using ConvertTextInAudio.Interfaces;
using System.Diagnostics;

namespace ConvertTextInAudio;
public class Worker(
    ILogger<Worker> logger,
    IGenerateAudioService service
    ) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var stopwatch = new Stopwatch();

        try
        {
            logger.LogInformation("Iniciando geração de áudio...");
            stopwatch.Start();

            var audioBytes = await service.GenerateAudioAsync("", stoppingToken);

            logger.LogInformation("Áudio gerado com sucesso em {ElapsedMilliseconds} ms", stopwatch.ElapsedMilliseconds);

            var outputPath = "records/voz.mp3";
            await File.WriteAllBytesAsync(outputPath, audioBytes, stoppingToken);

            logger.LogInformation("Arquivo salvo em: {OutputPath}", outputPath);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao gerar áudio");
        }
    }
}