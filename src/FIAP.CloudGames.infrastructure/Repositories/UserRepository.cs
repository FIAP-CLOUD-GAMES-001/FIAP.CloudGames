using FIAP.CloudGames.Domain.Entities;
using FIAP.CloudGames.Domain.Interfaces.Repositories;
using FIAP.CloudGames.infrastructure.Data;
using FIAP.CloudGames.infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FIAP.CloudGames.infrastructure.Repositories;
public class UserRepository : RepositoryBase<UserEntity>, IUserRepository
{
    public UserRepository(DataContext context) : base(context) { }
}