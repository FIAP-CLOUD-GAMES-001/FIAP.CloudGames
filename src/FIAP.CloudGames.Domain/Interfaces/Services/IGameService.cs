using FIAP.CloudGames.Domain.Requests.Game;
using FIAP.CloudGames.Domain.Responses.Game;

namespace FIAP.CloudGames.Domain.Interfaces.Services;
public interface IGameService
{
    Task<GameResponse> CreateAsync(CreateGameRequest request);
    Task<IEnumerable<GameResponse>> ListAsync();
}