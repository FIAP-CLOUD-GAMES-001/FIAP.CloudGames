using FIAP.CloudGames.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace FIAP.CloudGames.Domain.Entities;
public class UserEntity : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public ERole Role { get; private set; } = ERole.User;
    public ICollection<OwnedGameEntity> OwnedGames { get; private set; } = [];

    private UserEntity() { }
    public UserEntity(string name, string email, string plainPassword, ERole role = ERole.User)
    {
        Name = name.Trim();
        Email = email.Trim().ToLowerInvariant();
        Role = role;
        PasswordHash = HashPassword(plainPassword);
    }

    private string HashPassword(string plainPassword)
    {
        return new PasswordHasher<UserEntity>().HashPassword(this, plainPassword);
    }

    public bool VerifyPassword(string plainPassword)
    {
        var result = new PasswordHasher<UserEntity>()
            .VerifyHashedPassword(this, PasswordHash, plainPassword);
        return result == PasswordVerificationResult.Success;
    }

    public void UpdateRole(ERole role)
    {
        Role = role;
    }
}