namespace FIAP.CloudGames.Domain.Responses.Game;
public record OwnedGameResponse(int Id, int UserId, int GameId, DateTime PurchaseDate);