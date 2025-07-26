using FIAP.CloudGames.Domain.Entities;
using FIAP.CloudGames.Domain.Enums;
using FIAP.CloudGames.Domain.Exceptions;
using FIAP.CloudGames.Domain.Interfaces.Repositories;
using FIAP.CloudGames.Domain.Requests.Game;
using FIAP.CloudGames.Service.Game;
using Moq;

namespace FIAP.CloudGames.Test;

public class TesteServicoOwnedGame
{
    [Fact]
    public void Deve_Criar_OwnedGame_ComValoresCorretos()
    {
        var userId = 10;
        var gameId = 20;

        // Act
        var entity = new OwnedGameEntity(userId, gameId);

        Assert.Equal(userId, entity.UserId);
        Assert.Equal(gameId, entity.GameId);

        Assert.True(entity.PurchaseDate <= DateTime.UtcNow && entity.PurchaseDate > DateTime.UtcNow.AddMinutes(-1));

        Assert.Null(entity.User);
        Assert.Null(entity.Game);
    }

    [Fact]
    public async Task Deve_AdicionarOwnedGameERetornarResponse()
    {
        // Arrange
        var user = new UserEntity("Usuário", "email@teste.com", "Senha@123", ERole.User);
        var game = new GameEntity("Jogo", "Descrição", 10, EGameGenre.Action, DateTime.Today);

        var userRepoMock = new Mock<IUserRepository>();
        var gameRepoMock = new Mock<IGameRepository>();
        var ownedGameRepoMock = new Mock<IOwnedGameRepository>();

        userRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(user);
        gameRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(game);
        ownedGameRepoMock.Setup(r => r.AddAsync(It.IsAny<OwnedGameEntity>())).Returns(Task.CompletedTask);

        var service = new OwnedGameService(ownedGameRepoMock.Object, gameRepoMock.Object, userRepoMock.Object);

        var request = new AddOwnedGameRequest(1, 2);

        // Act
        var response = await service.AddAsync(request);

        // Assert
        Assert.Equal(request.UserId, response.UserId);
        Assert.Equal(request.GameId, response.GameId);
        Assert.True(response.PurchaseDate <= DateTime.UtcNow);
        ownedGameRepoMock.Verify(r => r.AddAsync(It.IsAny<OwnedGameEntity>()), Times.Once);
    }

    [Fact]
    public async Task Deve_LancarExcecao_SeUsuarioNaoEncontrado()
    {
        var userRepoMock = new Mock<IUserRepository>();
        var gameRepoMock = new Mock<IGameRepository>();
        var ownedGameRepoMock = new Mock<IOwnedGameRepository>();

        userRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((UserEntity?)null);

        var service = new OwnedGameService(ownedGameRepoMock.Object, gameRepoMock.Object, userRepoMock.Object);

        var request = new AddOwnedGameRequest(1, 2);

        await Assert.ThrowsAsync<NotFoundException>(() => service.AddAsync(request));
    }

    [Fact]
    public async Task Deve_LancarExcecao_SeGameNaoEncontrado()
    {
        var user = new UserEntity("Usuário", "email@teste.com", "Senha@123", ERole.User);

        var userRepoMock = new Mock<IUserRepository>();
        var gameRepoMock = new Mock<IGameRepository>();
        var ownedGameRepoMock = new Mock<IOwnedGameRepository>();

        userRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(user);
        gameRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((GameEntity?)null);

        var service = new OwnedGameService(ownedGameRepoMock.Object, gameRepoMock.Object, userRepoMock.Object);

        var request = new AddOwnedGameRequest(1, 2);

        await Assert.ThrowsAsync<NotFoundException>(() => service.AddAsync(request));
    }

    [Fact]
    public async Task Deve_RetornarOwnedGameResponse()
    {
        var ownedGames = new List<OwnedGameEntity>
        {
            new OwnedGameEntity(1, 2),
            new OwnedGameEntity(1, 3)
        };

        var userRepoMock = new Mock<IUserRepository>();
        var gameRepoMock = new Mock<IGameRepository>();
        var ownedGameRepoMock = new Mock<IOwnedGameRepository>();

        ownedGameRepoMock.Setup(r => r.GetByUserIdAsync(1)).ReturnsAsync(ownedGames);

        var service = new OwnedGameService(ownedGameRepoMock.Object, gameRepoMock.Object, userRepoMock.Object);

        var result = await service.GetByUserIdAsync(1);

        Assert.Collection(result,
            og => Assert.Equal(2, og.GameId),
            og => Assert.Equal(3, og.GameId)
        );
        ownedGameRepoMock.Verify(r => r.GetByUserIdAsync(1), Times.Once);
    }
}