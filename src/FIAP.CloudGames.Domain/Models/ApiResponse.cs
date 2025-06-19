namespace FIAP.CloudGames.Domain.Models;
public class ApiResponse<T>
{
    public bool Success { get; init; } = true;
    public string? Message { get; init; }
    public T? Data { get; init; }
    public List<string> Errors { get; init; } = [];
    public DateTime Timestamp { get; init; } = DateTime.UtcNow;

    public static ApiResponse<T> Ok(T data, string? message = null) =>
        new() { Data = data, Message = message };

    public static ApiResponse<T> Fail(string message, List<string>? errors = null) =>
        new() { Success = false, Message = message, Errors = errors ?? [] };
}