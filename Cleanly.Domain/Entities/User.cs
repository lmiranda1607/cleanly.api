using Cleanly.Domain.Enums;

namespace Cleanly.Domain.Entities;

public class User
{
    private readonly List<Address> _addresses = [];

    private User()
    {
    }

    public User(string fullName, string email, string phone, UserRole role)
    {
        FullName = fullName;
        Email = email;
        Phone = phone;
        Role = role;
        ReputationScore = 5.0m;
        CreatedAt = DateTimeOffset.UtcNow;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public long Id { get; private set; }
    public string FullName { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string Phone { get; private set; } = string.Empty;
    public UserRole Role { get; private set; }
    public decimal ReputationScore { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }

    public CustomerProfile? CustomerProfile { get; private set; }
    public CleanerProfile? CleanerProfile { get; private set; }
    public IReadOnlyCollection<Address> Addresses => _addresses;

    public void UpdateProfile(string fullName, string phone)
    {
        FullName = fullName;
        Phone = phone;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void SetReputationScore(decimal score)
    {
        ReputationScore = score;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}
