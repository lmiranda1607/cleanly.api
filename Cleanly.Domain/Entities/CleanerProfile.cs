namespace Cleanly.Domain.Entities;

public class CleanerProfile
{
    private CleanerProfile()
    {
    }

    public CleanerProfile(long userId, string? bio, int serviceRadiusKm)
    {
        UserId = userId;
        Bio = bio;
        Active = true;
        ServiceRadiusKm = serviceRadiusKm;
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public long UserId { get; private set; }
    public string? Bio { get; private set; }
    public bool Active { get; private set; }
    public int ServiceRadiusKm { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }

    public User User { get; private set; } = null!;

    public CleanerLocation? CurrentLocation { get; private set; }

    public void UpdateDetails(string? bio, int serviceRadiusKm)
    {
        Bio = bio;
        ServiceRadiusKm = serviceRadiusKm;
    }

    public void Activate() => Active = true;
    public void Deactivate() => Active = false;
}
