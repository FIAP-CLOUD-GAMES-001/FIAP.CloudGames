using FIAP.CloudGames.Domain.Entities;
using FIAP.CloudGames.Domain.Enums;
using FIAP.CloudGames.Domain.Exceptions;
using FIAP.CloudGames.Domain.Interfaces.Auth;
using FIAP.CloudGames.Domain.Interfaces.Repositories;
using FIAP.CloudGames.Domain.Requests.Auth;
using FIAP.CloudGames.Service.Auth;
using Moq;

namespace FIAP.CloudGames.Test;

public class TesteServicoAutenticacao
{
    [Fact]
    public async Task Deve_RetornarToken_QuandoCredenciaisForemValidas()
    {
        var usuario = new UserEntity("Teste", "teste@email.com", "Senha@123", ERole.User);

        var mockRepo = new Mock<IUserRepository>();
        mockRepo.Setup(r => r.GetByEmailAsync(usuario.Email)).ReturnsAsync(usuario);

        var mockTokenService = new Mock<ITokenService>();
        mockTokenService.Setup(t => t.Generate(usuario)).Returns("fake-jwt-token");
        mockTokenService.Setup(t => t.GetExpirationDate("fake-jwt-token")).Returns(DateTime.UtcNow.AddHours(2));

        var service = new AuthService(mockRepo.Object, mockTokenService.Object);

        var request = new LoginRequest(usuario.Email, "Senha@123");

        var response = await service.LoginAsync(request);

        Assert.NotNull(response);
        Assert.Equal("fake-jwt-token", response.Token);
        Assert.True(response.ExpireIn > DateTime.UtcNow);
    }

    [Fact]
    public async Task Deve_LancarExcecao_QuandoEmailNaoEncontrado()
    {
        var mockRepo = new Mock<IUserRepository>();
        mockRepo.Setup(r => r.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync((UserEntity?)null);

        var mockTokenService = new Mock<ITokenService>();

        var service = new AuthService(mockRepo.Object, mockTokenService.Object);

        var request = new LoginRequest("naoexiste@email.com", "Senha@123");

        await Assert.ThrowsAsync<AuthenticationException>(() => service.LoginAsync(request));
    }

    [Fact]
    public async Task Deve_LancarExcecao_QuandoSenhaIncorreta()
    {
        var usuario = new UserEntity("Teste", "teste@email.com", "Senha@123", ERole.User);

        var mockRepo = new Mock<IUserRepository>();
        mockRepo.Setup(r => r.GetByEmailAsync(usuario.Email)).ReturnsAsync(usuario);

        var mockTokenService = new Mock<ITokenService>();

        var service = new AuthService(mockRepo.Object, mockTokenService.Object);

        var request = new LoginRequest(usuario.Email, "SenhaErrada");

        await Assert.ThrowsAsync<AuthenticationException>(() => service.LoginAsync(request));
    }
}