namespace FIAP.CloudGames.Domain.Responses.Auth;
public record AuthResponse(string? Token, DateTime? ExpireIn);