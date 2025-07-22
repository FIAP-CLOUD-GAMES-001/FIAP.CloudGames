using FIAP.CloudGames.Domain.Exceptions;
using FIAP.CloudGames.Domain.Interfaces.Auth;
using FIAP.CloudGames.Domain.Interfaces.Repositories;
using FIAP.CloudGames.Domain.Interfaces.Services;
using FIAP.CloudGames.Domain.Requests.Auth;
using FIAP.CloudGames.Domain.Responses.Auth;

namespace FIAP.CloudGames.Service.Auth;

public class AuthService(IUserRepository repository, ITokenService tokens) : IAuthService
{
    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var user = await repository.GetByEmailAsync(request.Email)
                   ?? throw new AuthenticationException("Credenciais inválidas.");

        var passwordOk = user.VerifyPassword(request.Password);

        if (!passwordOk)
            throw new AuthenticationException("Credenciais inválidas.");

        var token = tokens.Generate(user);
        var validTo = tokens.GetExpirationDate(token);

        return new AuthResponse(token, validTo);
    }
}