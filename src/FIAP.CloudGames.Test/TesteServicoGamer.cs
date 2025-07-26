using FIAP.CloudGames.Domain.Entities;
using FIAP.CloudGames.Domain.Enums;
using FIAP.CloudGames.Domain.Interfaces.Repositories;
using FIAP.CloudGames.Domain.Requests.Game;
using FIAP.CloudGames.Service.Game;
using Moq;

namespace FIAP.CloudGames.Test;

public class TesteServicoGamer
{
    [Fact]
    public void Deve_Criar_GameEntity_Com_Valores_Corretos()
    {
        // Arrange
        var title = "  Teste  ";
        var description = "  Descrição  ";
        var price = 99.99m;
        var genre = EGameGenre.RPG;
        var releaseDate = new DateTime(2024, 6, 1);

        // Act
        var game = new GameEntity(title, description, price, genre, releaseDate);

        // Assert
        Assert.Equal("Teste", game.Title);
        Assert.Equal("Descrição", game.Description);
        Assert.Equal(price, game.Price);
        Assert.Equal(genre, game.Genre);
        Assert.Equal(releaseDate, game.ReleaseDate);
        Assert.NotNull(game.OwnedByUsers);
        Assert.Empty(game.OwnedByUsers);
    }

    [Fact]
    public void Deve_Atualizar_PrecoDoGame()
    {
        // Arrange
        var game = new GameEntity("Jogo", "Desc", 10m, EGameGenre.Action, DateTime.Today);

        // Act
        game.UpdatePrice(25.5m);

        // Assert
        Assert.Equal(25.5m, game.Price);
    }

    [Fact]
    public async Task Deve_Cadastrar_Game_Com_Sucesso()
    {
        // Arrange
        var repoMock = new Mock<IGameRepository>();
        var request = new CreateGameRequest("Novo Jogo", "Descrição", 99.99m, EGameGenre.Action, new DateTime(2024, 6, 1));

        repoMock.Setup(r => r.AddAsync(It.IsAny<GameEntity>()))
                .Returns(Task.CompletedTask);

        var service = new GameService(repoMock.Object);

        // Act
        var response = await service.CreateAsync(request);

        // Assert
        Assert.Equal(request.Title.Trim(), response.Title);
        Assert.Equal(request.Description.Trim(), response.Description);
        Assert.Equal(request.Price, response.Price);
        Assert.Equal(request.Genre, response.Genre);
        Assert.Equal(request.ReleaseDate, response.ReleaseDate);
        repoMock.Verify(r => r.AddAsync(It.IsAny<GameEntity>()), Times.Once);
    }

    [Fact]
    public async Task Deve_RetornarListaDeGameResponse()
    {
        // Arrange
        var repoMock = new Mock<IGameRepository>();
        var games = new List<GameEntity>
        {
            new GameEntity("Jogo 1", "Desc 1", 10, EGameGenre.RPG, DateTime.Today),
            new GameEntity("Jogo 2", "Desc 2", 20, EGameGenre.Action, DateTime.Today)
        };

        repoMock.Setup(r => r.ListAllAsync())
                .ReturnsAsync(games);

        var service = new GameService(repoMock.Object);

        // Act
        var result = await service.ListAsync();

        // Assert
        Assert.Collection(result,
            g =>
            {
                Assert.Equal("Jogo 1", g.Title);
                Assert.Equal(10, g.Price);
            },
            g =>
            {
                Assert.Equal("Jogo 2", g.Title);
                Assert.Equal(20, g.Price);
            }
        );
        repoMock.Verify(r => r.ListAllAsync(), Times.Once);
    }
}