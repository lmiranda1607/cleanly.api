using Cleanly.Domain.Enums;
using Cleanly.Domain.Exceptions;

namespace Cleanly.Domain.Entities;

public class ServiceRequest
{
    private static readonly IReadOnlyDictionary<ServiceRequestStatus, ServiceRequestStatus> ForwardTransitions =
        new Dictionary<ServiceRequestStatus, ServiceRequestStatus>
        {
            [ServiceRequestStatus.Created] = ServiceRequestStatus.SearchingCleaner,
            [ServiceRequestStatus.SearchingCleaner] = ServiceRequestStatus.Matched,
            [ServiceRequestStatus.Matched] = ServiceRequestStatus.CleanerOnTheWay,
            [ServiceRequestStatus.CleanerOnTheWay] = ServiceRequestStatus.Arrived,
            [ServiceRequestStatus.Arrived] = ServiceRequestStatus.InProgress,
            [ServiceRequestStatus.InProgress] = ServiceRequestStatus.Completed
        };

    private ServiceRequest()
    {
    }

    public ServiceRequest(long customerId, long addressId, DateTimeOffset requestedDate, int durationHours, string? notes)
    {
        CustomerId = customerId;
        AddressId = addressId;
        RequestedDate = requestedDate;
        DurationHours = durationHours;
        Notes = notes;
        Status = ServiceRequestStatus.Created;
        CreatedAt = DateTimeOffset.UtcNow;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public long Id { get; private set; }
    public long CustomerId { get; private set; }
    public long AddressId { get; private set; }
    public DateTimeOffset RequestedDate { get; private set; }
    public int DurationHours { get; private set; }
    public string? Notes { get; private set; }
    public ServiceRequestStatus Status { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }

    public User Customer { get; private set; } = null!;
    public Address Address { get; private set; } = null!;
    public ServiceMatch? Match { get; private set; }
    public Rating? Rating { get; private set; }

    public void MoveTo(ServiceRequestStatus targetStatus)
    {
        if (Status == ServiceRequestStatus.Completed || Status == ServiceRequestStatus.Cancelled)
        {
            throw new InvalidServiceRequestTransitionException(Status, targetStatus);
        }

        var isCancellation = targetStatus == ServiceRequestStatus.Cancelled;
        if (isCancellation)
        {
            Status = ServiceRequestStatus.Cancelled;
            UpdatedAt = DateTimeOffset.UtcNow;
            return;
        }

        if (!ForwardTransitions.TryGetValue(Status, out var expectedNext) || expectedNext != targetStatus)
        {
            throw new InvalidServiceRequestTransitionException(Status, targetStatus);
        }

        Status = targetStatus;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}
