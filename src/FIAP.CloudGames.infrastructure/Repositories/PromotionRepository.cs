using FIAP.CloudGames.Domain.Entities;
using FIAP.CloudGames.Domain.Interfaces.Repositories;
using FIAP.CloudGames.infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FIAP.CloudGames.infrastructure.Repositories;
public class PromotionRepository(DataContext context) : IPromotionRepository
{
    public async Task AddAsync(PromotionEntity promotion)
    {
        await context.Promotions.AddAsync(promotion);
        await context.SaveChangesAsync();
    }

    public async Task<PromotionEntity?> GetByIdAsync(int id)
    {
        return await context.Promotions.FindAsync(id);
    }

    public async Task<List<PromotionEntity>> ListAllAsync()
    {
        return await context.Promotions.AsNoTracking().ToListAsync();
    }
}