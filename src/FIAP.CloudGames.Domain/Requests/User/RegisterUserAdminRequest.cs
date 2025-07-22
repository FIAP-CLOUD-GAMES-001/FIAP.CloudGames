using FIAP.CloudGames.Domain.Enums;

namespace FIAP.CloudGames.Domain.Requests.User;
public record RegisterUserAdminRequest(string Name, string Email, string Password, ERole Role = ERole.User);