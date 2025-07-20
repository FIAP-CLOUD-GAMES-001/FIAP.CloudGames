using FIAP.CloudGames.Domain.Entities;
using FIAP.CloudGames.Domain.Enums;
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
        var user = new UserEntity(request.Name, request.Email, request.Password, ERole.User);

        if (await repository.EmailExistsAsync(user.Email))
            throw new ConflictException("Usuário já cadastrado.");

        await repository.AddAsync(user);

        return new UserResponse(user.Id, user.Name, user.Email, user.Role);
    }

    public async Task<UserResponse> RegisterAdminAsync(RegisterUserAdminRequest request)
    {
        var user = new UserEntity(request.Name, request.Email, request.Password, request.Role);

        if (await repository.EmailExistsAsync(user.Email))
            throw new ConflictException("Usuário já cadastrado.");

        await repository.AddAsync(user);

        return new UserResponse(user.Id, user.Name, user.Email, user.Role);
    }

    public async Task<UserResponse> GetByIdAsync(int id)
    {
        var user = await repository.GetByIdAsync(id);

        return user is null
            ? throw new NotFoundException($"User with ID {id} was not found.")
            : new UserResponse(user.Id, user.Name, user.Email, user.Role);
    }

    public async Task<List<UserResponse>> GetAllUsersAsync()
    {
        var users = await repository.ListAllAsync();
        return [.. users.Select(x => new UserResponse(x.Id, x.Name, x.Email, x.Role))];
    }

    public async Task<UserResponse> UpdateUserRoleAsync(int userId, ERole newRole)
    {
        var user = await repository.GetByIdAsync(userId)
                   ?? throw new NotFoundException($"User with ID {userId} was not found.");

        user.UpdateRole(newRole);
        await repository.UpdateAsync(user);
        return new UserResponse(user.Id, user.Name, user.Email, user.Role);
    }
}