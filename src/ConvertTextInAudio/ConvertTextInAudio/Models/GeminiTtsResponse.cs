using ConvertTextInAudio.Integration;

namespace ConvertTextInAudio.Models;
public class GeminiTtsResponse
{
    public Candidate[] Candidates { get; set; } = [];
}