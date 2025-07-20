using FIAP.CloudGames.Domain.Enums;

namespace FIAP.CloudGames.Domain.Responses.Game;
public record GameResponse(int Id, string Title, string Description, decimal Price, EGameGenre Genre, DateTime ReleaseDate);