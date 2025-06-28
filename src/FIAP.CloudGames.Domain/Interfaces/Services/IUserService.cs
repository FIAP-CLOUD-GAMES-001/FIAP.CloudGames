using FIAP.CloudGames.Domain.Requests.User;
using FIAP.CloudGames.Domain.Responses.User;

namespace FIAP.CloudGames.Domain.Interfaces.Services;
public interface IUserService
{
    Task<UserResponse> RegisterAsync(RegisterUserRequest request, Guid idUser);

    public List<RegisterUserRequest> GetList();

    public RegisterUserRequest GetById(Guid id);

    public RegisterUserRequest GetByEmail(string email);

    public void Delete(Guid id);

    public RegisterUserRequest Update(RegisterUserRequest userEdit);
    
}