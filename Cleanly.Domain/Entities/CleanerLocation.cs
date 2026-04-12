namespace Cleanly.Domain.Entities;

public class CleanerLocation
{
    private CleanerLocation()
    {
    }

    public CleanerLocation(long cleanerId, decimal latitude, decimal longitude)
    {
        CleanerId = cleanerId;
        Latitude = latitude;
        Longitude = longitude;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public long CleanerId { get; private set; }
    public decimal Latitude { get; private set; }
    public decimal Longitude { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }

    public CleanerProfile CleanerProfile { get; private set; } = null!;

    public void Update(decimal latitude, decimal longitude)
    {
        Latitude = latitude;
        Longitude = longitude;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}
