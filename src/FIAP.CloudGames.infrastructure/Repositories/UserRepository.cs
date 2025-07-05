using FIAP.CloudGames.Domain.Entities;
using FIAP.CloudGames.Domain.Interfaces.Repositories;
using FIAP.CloudGames.infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FIAP.CloudGames.infrastructure.Repositories;
public class UserRepository(DataContext context) : IUserRepository
{
    public async Task AddAsync(UserEntity user)
    {
        await context.AddAsync(user);
        await context.SaveChangesAsync();
    }

    public Task<bool> EmailExistsAsync(string email)
    {
        return context.Users.AnyAsync(u => u.Email == email);
    }

    public Task<UserEntity?> GetByEmailAsync(string email)
    {
        return context.Users.AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<List<UserEntity>> ListAllAsync()
    {
        return await context.Users
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<UserEntity?> GetByIdAsync(int id)
    {
        return await context.Users
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task UpdateAsync(UserEntity user)
    {
        context.Users.Update(user);
        await context.SaveChangesAsync();
    }
}