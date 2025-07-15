using FIAP.CloudGames.Domain.Entities;

namespace FIAP.CloudGames.Domain.Interfaces.Auth
{
    public interface ITokenService
    {
        string Generate(UserEntity usuario);
        DateTime GetExpirationDate(string token);
    }
}
