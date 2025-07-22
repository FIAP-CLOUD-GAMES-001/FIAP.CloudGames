using FIAP.CloudGames.Domain.Enums;
using FIAP.CloudGames.Domain.Requests.User;
using FIAP.CloudGames.Domain.Responses.User;

namespace FIAP.CloudGames.Domain.Interfaces.Services;
public interface IUserService
{
    Task<UserResponse> GetByIdAsync(int id);
    Task<UserResponse> RegisterAsync(RegisterUserRequest request);
    Task<UserResponse> RegisterAdminAsync(RegisterUserAdminRequest request);
    Task<List<UserResponse>> GetAllUsersAsync();
    Task<UserResponse> UpdateUserRoleAsync(int userId, ERole newRole);
}