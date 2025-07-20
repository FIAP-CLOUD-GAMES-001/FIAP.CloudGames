namespace FIAP.CloudGames.Domain.Entities;
public class PromotionEntity : BaseEntity
{
    public string Title { get; private set; } = string.Empty;
    public decimal DiscountPercentage { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }

    public int GameId { get; private set; }
    public GameEntity? Game { get; private set; }

    private PromotionEntity() { }

    public PromotionEntity(string title, decimal discount, DateTime start, DateTime end, int gameId)
    {
        Title = title.Trim();
        DiscountPercentage = discount;
        StartDate = start;
        EndDate = end;
        GameId = gameId;
    }
}