using FIAP.CloudGames.Domain.Requests.Auth;
using FIAP.CloudGames.Domain.Responses.Auth;

namespace FIAP.CloudGames.Domain.Interfaces.Services;
public interface IAuthService
{
    Task<AuthResponse> LoginAsync(LoginRequest request);
}