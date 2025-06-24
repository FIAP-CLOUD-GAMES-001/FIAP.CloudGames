using FIAP.CloudGames.Domain.Entities;
using FIAP.CloudGames.Domain.Exceptions;
using FIAP.CloudGames.Domain.Interfaces.Repositories;
using FIAP.CloudGames.Domain.Interfaces.Services;
using FIAP.CloudGames.Domain.Requests.User;
using FIAP.CloudGames.Domain.Responses.User;

namespace FIAP.CloudGames.Service.User;
public class UserService(IUserRepository repository) : IUserService
{
    public async Task<UserResponse> RegisterAsync(RegisterUserRequest request)
    {
        var user = new UserEntity(request.Name, request.Email, request.Password);

        if (await repository.EmailExistsAsync(user.Email))
            throw new ConflictException("Usuário já cadastrado.");

        await repository.AddAsync(user);

        return new UserResponse(user.Id, user.Name, user.Email);
    }
}