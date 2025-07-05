using FIAP.CloudGames.Domain.Entities;

namespace FIAP.CloudGames.Domain.Interfaces.Repositories;
public interface IUserRepository
{
    Task<bool> EmailExistsAsync(string email);
    Task<UserEntity?> GetByEmailAsync(string email);
    Task AddAsync(UserEntity user);
    Task<List<UserEntity>> ListAllAsync();
}