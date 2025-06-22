using FIAP.CloudGames.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace FIAP.CloudGames.Domain.Entities;
public class User
{
    public int Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public Role Role { get; private set; } = Role.User;
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    private User() { }
    public User(string name, string email, string plainPassword, Role role = Role.User)
    {
        Name = name.Trim();
        Email = email.Trim().ToLowerInvariant();
        Role = role;
        PasswordHash = HashPassword(plainPassword);
    }

    private string HashPassword(string plainPassword)
    {
        return new PasswordHasher<User>().HashPassword(this, plainPassword);
    }

    public bool VerifyPassword(string plainPassword)
    {
        var result = new PasswordHasher<User>()
            .VerifyHashedPassword(this, PasswordHash, plainPassword);
        return result == PasswordVerificationResult.Success;
    }
}