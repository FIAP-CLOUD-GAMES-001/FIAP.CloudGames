using FIAP.CloudGames.Domain.Entities;
using FIAP.CloudGames.Domain.Exceptions;
using FIAP.CloudGames.Domain.Interfaces.Repositories;
using FIAP.CloudGames.Domain.Interfaces.Services;
using FIAP.CloudGames.Domain.Requests.Game;
using FIAP.CloudGames.Domain.Responses.Game;

namespace FIAP.CloudGames.Service.Game;
public class PromotionService(
    IPromotionRepository promotionRepository,
    IGameRepository gameRepository) : IPromotionService
{
    public async Task<PromotionResponse> CreateAsync(CreatePromotionRequest request)
    {
        var game = await gameRepository.GetByIdAsync(request.GameId)
            ?? throw new NotFoundException($"Game with ID {request.GameId} not found.");

        var promotion = new PromotionEntity(request.Title, request.DiscountPercentage, request.StartDate, request.EndDate, request.GameId);
        await promotionRepository.AddAsync(promotion);
        return new PromotionResponse(promotion.Id, promotion.Title, promotion.DiscountPercentage, promotion.StartDate, promotion.EndDate, promotion.GameId);
    }

    public async Task<IEnumerable<PromotionResponse>> ListAsync()
    {
        var promotions = await promotionRepository.ListAllAsync();
        return promotions.Select(p => new PromotionResponse(p.Id, p.Title, p.DiscountPercentage, p.StartDate, p.EndDate, p.GameId));
    }
}