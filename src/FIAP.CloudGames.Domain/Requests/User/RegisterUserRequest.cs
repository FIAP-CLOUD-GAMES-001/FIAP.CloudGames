using FIAP.CloudGames.Domain.Enums;

namespace FIAP.CloudGames.Domain.Requests.User;
public record RegisterUserRequest(string Name, string Email, string Password, Role Role = Role.User);