using FIAP.CloudGames.Domain.Requests.User;
using FIAP.CloudGames.Domain.Responses.User;

namespace FIAP.CloudGames.Domain.Interfaces.Services;
public interface IUserService
{
    Task<UserResponse> RegisterAsync(RegisterUserRequest request);
}