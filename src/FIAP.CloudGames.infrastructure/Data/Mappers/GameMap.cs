using FIAP.CloudGames.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FIAP.CloudGames.infrastructure.Data.Mappers;
public class GameMap : IEntityTypeConfiguration<GameEntity>
{
    public void Configure(EntityTypeBuilder<GameEntity> builder)
    {
        builder.ToTable("Games").HasKey(g => g.Id);

        builder.Property(g => g.Id)
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(g => g.Title)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(g => g.Description)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(g => g.Price)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(g => g.Genre)
            .IsRequired();

        builder.Property(g => g.ReleaseDate)
            .IsRequired();

        builder.Property(g => g.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
    }
}