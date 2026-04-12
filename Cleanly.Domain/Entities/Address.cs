namespace Cleanly.Domain.Entities;

public class Address
{
    private Address()
    {
    }

    public Address(
        long userId,
        string label,
        string street,
        string? number,
        string? neighborhood,
        string city,
        string state,
        string? zipCode,
        decimal? latitude,
        decimal? longitude)
    {
        UserId = userId;
        Label = label;
        Street = street;
        Number = number;
        Neighborhood = neighborhood;
        City = city;
        State = state;
        ZipCode = zipCode;
        Latitude = latitude;
        Longitude = longitude;
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public long Id { get; private set; }
    public long UserId { get; private set; }
    public string Label { get; private set; } = string.Empty;
    public string Street { get; private set; } = string.Empty;
    public string? Number { get; private set; }
    public string? Neighborhood { get; private set; }
    public string City { get; private set; } = string.Empty;
    public string State { get; private set; } = string.Empty;
    public string? ZipCode { get; private set; }
    public decimal? Latitude { get; private set; }
    public decimal? Longitude { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }

    public User User { get; private set; } = null!;
}
