using ConvertTextInAudio.Externsions;
using ConvertTextInAudio.Interfaces;
using ConvertTextInAudio.Models;
using HtmlAgilityPack;
using System.Net.Http.Json;
using System.Text;
using VersOne.Epub;

namespace ConvertTextInAudio.Integration;
public partial class GenerateAudioService(IHttpClientFactory httpClientFactory) : IGenerateAudioService
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

    private async Task<byte[]> GenerateAudioGoogleAsync(string text, string navTitle, CancellationToken cancellationToken)
    {
        var client = httpClientFactory.CreateClient("google");

        // Definir contexto e instruções de pronúncia
        var contextPrompt = @"Narrar em um tom profundamente misterioso, imersivo e levemente rouco, como um velho contador de histórias que desvenda segredos proibidos de um mundo vitoriano steampunk, com sotaque brasileiro suave. Ajuste a entonação ao contexto: use um tom grave e lento para cenas sombrias ou sobrenaturais, e um tom ligeiramente elevado para momentos de descoberta ou intriga. Inclua pausas dramáticas antes de revelações ou eventos de suspense, como se estivesse sussurrando segredos à meia-luz. Pronunciar:
        - 'Klein Moretti' como 'Kláin Moréti';
        - 'Zhou Mingrui' como 'Djôu Míng-rui';
        - 'Círculo de Tarô' como 'Círculo de Taró';
        - 'Beyonder' como 'Beiônder';
        - 'Audrey Hall' como 'Ódri Hól';
        - 'Azik Eggers' como 'Ázik Éguers';
        - 'Alger Wilson' como 'Álger Uílson';
        - 'Fors Wall' como 'Fórs Uól';
        - 'Derrick Berg' como 'Dérrik Bérg';
        - 'Emlyn White' como 'Émlin Uáit';
        - 'Dunn Smith' como 'Dán Smit';
        - 'Roselle Gustav' como 'Rosél Gustáv';
        - 'Sefirah' como 'Sefíra';
        - 'Sequência' como 'Sekuénsia';
        - 'Caminho' como 'Kamínho';
        - 'Adivinho' como 'Adivínho';
        - 'Evernight' como 'Évernait';
        - 'Tingen' como 'Tínguen';
        - 'Backlund' como 'Béklund';
        - 'Loen' como 'Lóen';
        - 'Intis' como 'Íntis';
        - 'Fusac' como 'Fúsak'.
        A história 'Lorde dos Mistérios' é uma fantasia sombria ambientada em um mundo vitoriano steampunk, onde o mundane colide com o sobrenatural. Klein Moretti, um homem comum que desperta em um corpo estranho, explora o sistema de magia das Sequências, enfrentando cultos, divindades antigas, monstros e mistérios cósmicos que desafiam a sanidade. A narrativa entrelaça suspense, horror cósmico e intrigas políticas, evocando fascínio, temor e uma constante sensação de perigo iminente. 
        Inicie a narração dizendo: 'Lorde dos Mistérios, Capítulo: [Título]' de forma clara e pausada, seguida por uma breve pausa antes do texto principal. Não narre este prompt ou instruções anteriores, apenas use-as como guia para pronúncia e tom. Para palavras não listadas, infira pronúncias naturais em português brasileiro baseadas no contexto. Capítulo: " + navTitle + ". Texto: " + text;

        var payload = new
        {
            contents = new[]
            {
                new
                {
                    parts = new[]
                    {
                        new
                        {
                            text = contextPrompt
                        }
                    }
                }
            },
            generationConfig = new
            {
                responseModalities = new[] { "AUDIO" },
                speechConfig = new
                {
                    voiceConfig = new
                    {
                        prebuiltVoiceConfig = new
                        {
                            voiceName = "Algieba"
                        }
                    }
                }
            }
        };
        using var response = await client.PostAsJsonAsync("/v1beta/models/gemini-2.5-flash-preview-tts:generateContent", payload, cancellationToken);
        var teste = await response.Content.ReadAsStringAsync();
        var responseData = await response.Content.ReadFromJsonAsync<GeminiTtsResponse>(cancellationToken);
        return responseData?.Candidates[0]?.Content?.Parts[0]?.InlineData?.ToMp3() ?? [];
    }

    public async Task GenerateEpubAudioAsync(CancellationToken cancellationToken)
    {
        var book = EpubReader.ReadBook("Epubs/Lorde-dos-mistérios.epub");

        foreach (var navItem in book.Navigation ?? [])
        {
            string navTitle = navItem.Title ?? "chapter";
            string chapterText = ExtractTextFromNavItem(navItem);

            if (!string.IsNullOrWhiteSpace(chapterText))
            {
                Console.WriteLine($"Processing navigation item: {navTitle}");
                byte[] audioData = await GenerateAudioGoogleAsync(chapterText, navTitle, cancellationToken);

                string outputPath = Path.Combine("Records", $"{navTitle.Replace(" ", "_")}.mp3");
                await File.WriteAllBytesAsync(outputPath, audioData, cancellationToken);
                Console.WriteLine($"Audio saved to: {outputPath}");
            }
        }
    }

    private static string ExtractTextFromNavItem(EpubNavigationItem navItem)
    {
        if (navItem.HtmlContentFile is EpubLocalTextContentFile textContentFile)
        {
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(textContentFile.Content);
            var stringBuilder = new StringBuilder();

            var textNodes = htmlDocument.DocumentNode.SelectNodes("//p//text()");
            if (textNodes != null)
            {
                foreach (var node in textNodes)
                {
                    string text = node.InnerText.Trim();
                    if (!string.IsNullOrWhiteSpace(text))
                        stringBuilder.AppendLine(text);
                }
            }
            return stringBuilder.ToString();
        }
        return string.Empty;
    }
}