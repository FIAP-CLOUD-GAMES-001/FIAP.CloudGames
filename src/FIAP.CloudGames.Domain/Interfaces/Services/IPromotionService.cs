using FIAP.CloudGames.Domain.Requests.Game;
using FIAP.CloudGames.Domain.Responses.Game;

namespace FIAP.CloudGames.Domain.Interfaces.Services;
public interface IPromotionService
{
    Task<PromotionResponse> CreateAsync(CreatePromotionRequest request);
    Task<IEnumerable<PromotionResponse>> ListAsync();
}