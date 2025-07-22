using FIAP.CloudGames.Domain.Entities;
using FIAP.CloudGames.Domain.Enums;
using FIAP.CloudGames.Domain.Exceptions;
using FIAP.CloudGames.Domain.Interfaces.Repositories;
using FIAP.CloudGames.Domain.Requests.User;
using FIAP.CloudGames.Service.User;
using Moq;

namespace FIAP.CloudGames.Test;

public class TesteServicoUsuario
{
    [Fact]
    public void Deve_Criar_UserEntity_Com_Valores_Corretos()
    {
        var usuario = new UserEntity("João", "joao@email.com", "senha123", ERole.Admin);

        Assert.Equal("João", usuario.Name);
        Assert.Equal("joao@email.com", usuario.Email);
        Assert.Equal(ERole.Admin, usuario.Role);
        Assert.NotNull(usuario.PasswordHash);
        Assert.True(usuario.CreatedAt <= DateTime.UtcNow);
    }

    [Fact]
    public void Deve_Verificar_Senha_Correta()
    {
        var usuario = new UserEntity("Maria", "maria@email.com", "minhasenha", ERole.User);

        Assert.True(usuario.VerifyPassword("minhasenha"));
    }

    [Fact]
    public void Deve_Falhar_Verificacao_Senha_Incorreta()
    {
        var usuario = new UserEntity("Carlos", "carlos@email.com", "abc123", ERole.User);

        Assert.False(usuario.VerifyPassword("*pserrada*"));
    }

    [Fact]
    public void Deve_Atualizar_Role_Do_Usuario()
    {
        var usuario = new UserEntity("Ana", "ana@email.com", "senha456", ERole.User);

        usuario.UpdateRole(ERole.Admin);

        Assert.Equal(ERole.Admin, usuario.Role);
    }

    [Fact]
    public async Task Deve_Cadastrar_Usuario_Com_Sucesso()
    {
        // Arrange
        var mockRepo = new Mock<IUserRepository>();
        mockRepo.Setup(r => r.EmailExistsAsync(It.IsAny<string>())).ReturnsAsync(false);
        mockRepo.Setup(r => r.AddAsync(It.IsAny<UserEntity>())).Returns(Task.CompletedTask);

        var service = new UserService(mockRepo.Object);

        var request = new RegisterUserRequest("Teste", "teste@email.com", "Senha@123");

        // Act
        var response = await service.RegisterAsync(request);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(request.Name, response.Name);
        Assert.Equal(request.Email.ToLowerInvariant(), response.Email);
        mockRepo.Verify(r => r.AddAsync(It.IsAny<UserEntity>()), Times.Once);
    }


    [Fact]
    public async Task Deve_Lancar_Excecao_Quando_Email_Ja_Estiver_Cadastrado()
    {
        // Arrange
        var mockRepo = new Mock<IUserRepository>();
        mockRepo.Setup(r => r.EmailExistsAsync("teste@email.com")).ReturnsAsync(true);

        var service = new UserService(mockRepo.Object);

        var request = new RegisterUserRequest("Teste", "teste@email.com", "Senha@123");

        // Act & Assert
        var ex = await Assert.ThrowsAsync<ConflictException>(() =>
        service.RegisterAsync(request));

        Assert.Equal("Usuário já cadastrado.", ex.Message);
        mockRepo.Verify(r => r.AddAsync(It.IsAny<UserEntity>()), Times.Never);
    }
}