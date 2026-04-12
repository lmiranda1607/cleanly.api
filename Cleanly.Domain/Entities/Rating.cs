namespace Cleanly.Domain.Entities;

public class Rating
{
    private Rating()
    {
    }

    public Rating(long requestId, long customerId, long cleanerId, int score, string? comment)
    {
        RequestId = requestId;
        CustomerId = customerId;
        CleanerId = cleanerId;
        Score = score;
        Comment = comment;
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public long Id { get; private set; }
    public long RequestId { get; private set; }
    public long CustomerId { get; private set; }
    public long CleanerId { get; private set; }
    public int Score { get; private set; }
    public string? Comment { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }

    public ServiceRequest Request { get; private set; } = null!;
    public User Customer { get; private set; } = null!;
    public User Cleaner { get; private set; } = null!;
}
