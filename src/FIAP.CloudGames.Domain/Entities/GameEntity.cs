using FIAP.CloudGames.Domain.Enums;

namespace FIAP.CloudGames.Domain.Entities;
public class GameEntity : BaseEntity
{
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public decimal Price { get; private set; }
    public EGameGenre Genre { get; private set; }
    public DateTime ReleaseDate { get; private set; }
    public ICollection<OwnedGameEntity> OwnedByUsers { get; private set; } = [];

    private GameEntity() { }

    public GameEntity(string title, string description, decimal price, EGameGenre genre, DateTime releaseDate)
    {
        Title = title.Trim();
        Description = description.Trim();
        Price = price;
        Genre = genre;
        ReleaseDate = releaseDate;
    }

    public void UpdatePrice(decimal newPrice)
    {
        Price = newPrice;
    }
}