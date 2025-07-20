using FIAP.CloudGames.Domain.Enums;

namespace FIAP.CloudGames.Domain.Responses.User;
public record UserResponse(int Id, string Name, string Email, ERole Role = ERole.User);