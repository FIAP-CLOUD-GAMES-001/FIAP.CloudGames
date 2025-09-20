namespace ConvertTextInAudio.Interfaces;
public interface IGenerateAudioService
{
    Task<byte[]> GenerateAudioAsync(string text, CancellationToken cancellationToken);
    Task GenerateEpubAudioAsync(CancellationToken cancellationToken);
}