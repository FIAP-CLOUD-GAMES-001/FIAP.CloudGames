using FIAP.CloudGames.Domain.Enums;

namespace FIAP.CloudGames.Domain.Requests.Game;
public record CreateGameRequest(string Title, string Description, decimal Price, EGameGenre Genre, DateTime ReleaseDate);