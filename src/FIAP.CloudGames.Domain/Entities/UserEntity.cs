using FIAP.CloudGames.Domain.Enums;
using Isopoh.Cryptography.Argon2;

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

    private static string HashPassword(string plainPassword)
    {
        return Argon2.Hash(plainPassword);
    }

    public bool VerifyPassword(string plainPassword)
    {
        return Argon2.Verify(PasswordHash, plainPassword);
    }

    public void UpdateRole(ERole role)
    {
        Role = role;
    }
}