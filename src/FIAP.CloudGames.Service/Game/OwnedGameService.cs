using FIAP.CloudGames.Domain.Entities;
using FIAP.CloudGames.Domain.Exceptions;
using FIAP.CloudGames.Domain.Interfaces.Repositories;
using FIAP.CloudGames.Domain.Interfaces.Services;
using FIAP.CloudGames.Domain.Requests.Game;
using FIAP.CloudGames.Domain.Responses.Game;

namespace FIAP.CloudGames.Service.Game;
public class OwnedGameService(
    IOwnedGameRepository ownedGameRepository,
    IGameRepository gameRepository,
    IUserRepository userRepository) : IOwnedGameService
{
    public async Task<OwnedGameResponse> AddAsync(AddOwnedGameRequest request)
    {
        var user = await userRepository.GetByIdAsync(request.UserId)
            ?? throw new NotFoundException($"User with ID {request.UserId} not found.");

        var game = await gameRepository.GetByIdAsync(request.GameId)
            ?? throw new NotFoundException($"Game with ID {request.GameId} not found.");

        var ownedGame = new OwnedGameEntity(request.UserId, request.GameId);
        await ownedGameRepository.AddAsync(ownedGame);
        return new OwnedGameResponse(ownedGame.Id, ownedGame.UserId, ownedGame.GameId, ownedGame.PurchaseDate);
    }

    public async Task<IEnumerable<OwnedGameResponse>> GetByUserIdAsync(int userId)
    {
        var ownedGames = await ownedGameRepository.GetByUserIdAsync(userId);
        return ownedGames.Select(og => new OwnedGameResponse(og.Id, og.UserId, og.GameId, og.PurchaseDate));
    }
}