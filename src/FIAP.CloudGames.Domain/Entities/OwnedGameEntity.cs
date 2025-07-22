namespace FIAP.CloudGames.Domain.Entities;
public class OwnedGameEntity : BaseEntity
{
    public int UserId { get; private set; }
    public UserEntity? User { get; private set; }

    public int GameId { get; private set; }
    public GameEntity? Game { get; private set; }

    public DateTime PurchaseDate { get; private set; }

    private OwnedGameEntity() { }

    public OwnedGameEntity(int userId, int gameId)
    {
        UserId = userId;
        GameId = gameId;
        PurchaseDate = DateTime.UtcNow;
    }
}