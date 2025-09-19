using ConvertTextInAudio.Interfaces;

namespace ConvertTextInAudio;
public class Worker(
    ILogger<Worker> logger,
    IGenerateAudioService service
    ) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            logger.LogInformation("Iniciando gera��o de �udio");
            var audioBytes = await service.GenerateAudioAsync("Feliz Natal", stoppingToken);

            logger.LogInformation("�udio gerado com sucesso");
            await File.WriteAllBytesAsync("voz.mp3", audioBytes, stoppingToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao gerar �udio");
        }
    }
}