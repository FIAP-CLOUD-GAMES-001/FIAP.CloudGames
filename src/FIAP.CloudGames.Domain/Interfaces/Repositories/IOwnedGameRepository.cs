using FIAP.CloudGames.Domain.Entities;

namespace FIAP.CloudGames.Domain.Interfaces.Repositories;
public interface IOwnedGameRepository
{
    Task AddAsync(OwnedGameEntity ownedGame);
    Task<List<OwnedGameEntity>> GetByUserIdAsync(int userId);
}