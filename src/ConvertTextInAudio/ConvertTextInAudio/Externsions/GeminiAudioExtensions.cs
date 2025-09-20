using ConvertTextInAudio.Models;
using NAudio.Lame;
using NAudio.Wave;

namespace ConvertTextInAudio.Externsions;
public static class GeminiAudioExtensions
{
    /// <summary>
    /// Converte o inlineData do Gemini (PCM base64) em MP3 bytes.
    /// </summary>
    public static byte[] ToMp3(this InlineData inlineData)
    {
        // 1. Decodifica Base64
        var pcmData = Convert.FromBase64String(inlineData.Data);

        // 2. Cria stream com PCM
        using var pcmStream = new MemoryStream(pcmData);

        // 3. Monta WaveFormat (16-bit PCM 24000 Hz mono)
        // Gemini devolve "audio/L16;codec=pcm;rate=24000"
        var waveFormat = new WaveFormat(24000, 16, 1);

        using var rawSource = new RawSourceWaveStream(pcmStream, waveFormat);

        // 4. Converter para MP3 usando NAudio.Lame
        using var mp3Stream = new MemoryStream();
        using var writer = new LameMP3FileWriter(mp3Stream, rawSource.WaveFormat, LAMEPreset.STANDARD);
        rawSource.CopyTo(writer);

        return mp3Stream.ToArray();
    }

    /// <summary>
    /// Se quiser só WAV em vez de MP3.
    /// </summary>
    public static byte[] ToWav(this InlineData inlineData)
    {
        var pcmData = Convert.FromBase64String(inlineData.Data);
        var waveFormat = new WaveFormat(24000, 16, 1);

        using var pcmStream = new MemoryStream(pcmData);
        using var rawSource = new RawSourceWaveStream(pcmStream, waveFormat);
        using var wavStream = new MemoryStream();

        WaveFileWriter.WriteWavFileToStream(wavStream, rawSource);
        return wavStream.ToArray();
    }
}