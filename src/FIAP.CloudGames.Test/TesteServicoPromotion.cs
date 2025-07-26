using FIAP.CloudGames.Domain.Entities;
using FIAP.CloudGames.Domain.Enums;
using FIAP.CloudGames.Domain.Exceptions;
using FIAP.CloudGames.Domain.Interfaces.Repositories;
using FIAP.CloudGames.Domain.Requests.Game;
using FIAP.CloudGames.Service.Game;
using Moq;

namespace FIAP.CloudGames.Test;

public class TesteServicoPromotion
{
    [Fact]
    public void Deve_Criar_PromotionComValoresCorretos()
    {
        var title = "Promoção";
        var discount = 0.25m;
        var start = DateTime.Today;
        var end = DateTime.Today.AddDays(5);
        var gameId = 1;

        var promo = new PromotionEntity(title, discount, start, end, gameId);

        Assert.Equal(title.Trim(), promo.Title);
        Assert.Equal(discount, promo.DiscountPercentage);
        Assert.Equal(start, promo.StartDate);
        Assert.Equal(end, promo.EndDate);
        Assert.Equal(gameId, promo.GameId);
    }

    [Fact]
    public async Task Deve_AdicionarPromocaoERetornarResponse()
    {
        // Arrange
        var promoRepoMock = new Mock<IPromotionRepository>();
        var gameRepoMock = new Mock<IGameRepository>();
        var game = new GameEntity("Jogo", "Desc", 10, EGameGenre.Action, DateTime.Today);

        gameRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(game);
        promoRepoMock.Setup(r => r.AddAsync(It.IsAny<PromotionEntity>())).Returns(Task.CompletedTask);

        var service = new PromotionService(promoRepoMock.Object, gameRepoMock.Object);

        var request = new CreatePromotionRequest("Promoção", 0.2m, DateTime.Today, DateTime.Today.AddDays(5), 1);
        
        // Act
        var response = await service.CreateAsync(request);

        // Assert
        Assert.Equal(request.Title.Trim(), response.Title);
        Assert.Equal(request.DiscountPercentage, response.DiscountPercentage);
        Assert.Equal(request.StartDate, response.StartDate);
        Assert.Equal(request.EndDate, response.EndDate);
        Assert.Equal(request.GameId, response.GameId);
        promoRepoMock.Verify(r => r.AddAsync(It.IsAny<PromotionEntity>()), Times.Once);
    }

    [Fact]
    public async Task Deve_LancarExcecao_SeGameNaoEncontrado()
    {
        var promoRepoMock = new Mock<IPromotionRepository>();
        var gameRepoMock = new Mock<IGameRepository>();
        gameRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((GameEntity?)null);

        var service = new PromotionService(promoRepoMock.Object, gameRepoMock.Object);

        var request = new CreatePromotionRequest("Promoção", 0.2m, DateTime.Today, DateTime.Today.AddDays(5), 1);

        await Assert.ThrowsAsync<NotFoundException>(() => service.CreateAsync(request));
    }

    [Fact]
    public async Task Deve_RetornarListaDePromotionResponse()
    {
        var promoRepoMock = new Mock<IPromotionRepository>();
        var gameRepoMock = new Mock<IGameRepository>();
        var promotions = new List<PromotionEntity>
        {
            new PromotionEntity("Promo 1", 0.1m, DateTime.Today, DateTime.Today.AddDays(1), 1),
            new PromotionEntity("Promo 2", 0.2m, DateTime.Today, DateTime.Today.AddDays(2), 2)
        };

        promoRepoMock.Setup(r => r.ListAllAsync()).ReturnsAsync(promotions);

        var service = new PromotionService(promoRepoMock.Object, gameRepoMock.Object);

        var result = await service.ListAsync();

        Assert.Collection(result,
            p => Assert.Equal("Promo 1", p.Title),
            p => Assert.Equal("Promo 2", p.Title)
        );
        promoRepoMock.Verify(r => r.ListAllAsync(), Times.Once);
    }
}