using Cleanly.Domain.Enums;

namespace Cleanly.Domain.Exceptions;

public sealed class InvalidServiceRequestTransitionException : Exception
{
    public InvalidServiceRequestTransitionException(ServiceRequestStatus currentStatus, ServiceRequestStatus targetStatus)
        : base($"Invalid service request transition from '{currentStatus}' to '{targetStatus}'.")
    {
        CurrentStatus = currentStatus;
        TargetStatus = targetStatus;
    }

    public ServiceRequestStatus CurrentStatus { get; }
    public ServiceRequestStatus TargetStatus { get; }
}
