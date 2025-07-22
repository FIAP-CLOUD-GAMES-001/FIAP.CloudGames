using FIAP.CloudGames.Domain.Requests.Game;
using FIAP.CloudGames.Domain.Responses.Game;

namespace FIAP.CloudGames.Domain.Interfaces.Services;
public interface IOwnedGameService
{
    Task<OwnedGameResponse> AddAsync(AddOwnedGameRequest request);
    Task<IEnumerable<OwnedGameResponse>> GetByUserIdAsync(int userId);
}