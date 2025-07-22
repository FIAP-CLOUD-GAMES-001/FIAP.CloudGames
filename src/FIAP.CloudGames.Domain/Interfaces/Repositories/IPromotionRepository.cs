using FIAP.CloudGames.Domain.Entities;

namespace FIAP.CloudGames.Domain.Interfaces.Repositories;
public interface IPromotionRepository
{
    Task AddAsync(PromotionEntity promotion);
    Task<PromotionEntity?> GetByIdAsync(int id);
    Task<List<PromotionEntity>> ListAllAsync();
}