namespace Cleanly.Domain.Entities;

public class CustomerProfile
{
    private CustomerProfile()
    {
    }

    public CustomerProfile(long userId, string? preferredPaymentMethod)
    {
        UserId = userId;
        PreferredPaymentMethod = preferredPaymentMethod;
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public long UserId { get; private set; }
    public string? PreferredPaymentMethod { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }

    public User User { get; private set; } = null!;

    public void UpdatePreferredPaymentMethod(string? preferredPaymentMethod)
    {
        PreferredPaymentMethod = preferredPaymentMethod;
    }
}
