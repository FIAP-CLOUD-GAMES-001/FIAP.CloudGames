using FIAP.CloudGames.Domain.Entities;
using FIAP.CloudGames.Domain.Interfaces.Repositories;
using FIAP.CloudGames.Domain.Interfaces.Services;
using FIAP.CloudGames.Domain.Requests.Game;
using FIAP.CloudGames.Domain.Responses.Game;

namespace FIAP.CloudGames.Service.Game;
public class GameService(IGameRepository gameRepository) : IGameService
{
    public async Task<GameResponse> CreateAsync(CreateGameRequest request)
    {
        var game = new GameEntity(request.Title, request.Description, request.Price, request.Genre, request.ReleaseDate);
        await gameRepository.AddAsync(game);
        return new GameResponse(game.Id, game.Title, game.Description, game.Price, game.Genre, game.ReleaseDate);
    }

    public async Task<IEnumerable<GameResponse>> ListAsync()
    {
        var games = await gameRepository.ListAllAsync();
        return games.Select(g => new GameResponse(g.Id, g.Title, g.Description, g.Price, g.Genre, g.ReleaseDate));
    }
}