namespace Cleanly.Domain.Entities;

public class ServiceMatch
{
    private ServiceMatch()
    {
    }

    public ServiceMatch(long requestId, long cleanerId)
    {
        RequestId = requestId;
        CleanerId = cleanerId;
        AcceptedAt = DateTimeOffset.UtcNow;
    }

    public long Id { get; private set; }
    public long RequestId { get; private set; }
    public long CleanerId { get; private set; }
    public DateTimeOffset AcceptedAt { get; private set; }
    public DateTimeOffset? StartedAt { get; private set; }
    public DateTimeOffset? FinishedAt { get; private set; }

    public ServiceRequest Request { get; private set; } = null!;
    public User Cleaner { get; private set; } = null!;

    public void MarkStarted() => StartedAt = DateTimeOffset.UtcNow;
    public void MarkFinished() => FinishedAt = DateTimeOffset.UtcNow;
}
