using FIAP.CloudGames.Domain.Entities;
using FIAP.CloudGames.Domain.Interfaces.Repositories;
using FIAP.CloudGames.infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FIAP.CloudGames.infrastructure.Repositories;
public class OwnedGameRepository(DataContext context) : IOwnedGameRepository
{
    public async Task AddAsync(OwnedGameEntity ownedGame)
    {
        await context.OwnedGames.AddAsync(ownedGame);
        await context.SaveChangesAsync();
    }

    public async Task<List<OwnedGameEntity>> GetByUserIdAsync(int userId)
    {
        return await context.OwnedGames
            .Where(og => og.UserId == userId)
            .Include(og => og.Game)
            .AsNoTracking()
            .ToListAsync();
    }
}