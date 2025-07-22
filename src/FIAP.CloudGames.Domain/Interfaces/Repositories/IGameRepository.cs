using FIAP.CloudGames.Domain.Entities;

namespace FIAP.CloudGames.Domain.Interfaces.Repositories;
public interface IGameRepository
{
    Task AddAsync(GameEntity game);
    Task<GameEntity?> GetByIdAsync(int id);
    Task<List<GameEntity>> ListAllAsync();
}