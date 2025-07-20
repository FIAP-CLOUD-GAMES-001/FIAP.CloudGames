using FIAP.CloudGames.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FIAP.CloudGames.infrastructure.Data;
/// <summary>
/// Geração dos migrations e scripts de banco de dados
/// </summary>
/// dotnet ef migrations add InitialCreate --project ../FIAP.CloudGames.infrastructure --startup-project ../FIAP.CloudGames.Api
/// dotnet ef migrations script -o ./Scripts/script_base.sql --context DataContext --startup-project ../FIAP.CloudGames.Api
/// dotnet ef database update --context DataContext --startup-project ../FIAP.CloudGames.Api
/// <param name="options"></param>
public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<UserEntity> Users { get; set; } = null!;
    public DbSet<GameEntity> Games { get; set; } = null!;
    public DbSet<PromotionEntity> Promotions { get; set; } = null!;
    public DbSet<OwnedGameEntity> OwnedGames { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);
    }
}