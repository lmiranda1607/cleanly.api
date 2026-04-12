namespace Cleanly.Domain.Enums;

public enum ServiceRequestStatus
{
    Created = 1,
    SearchingCleaner = 2,
    Matched = 3,
    CleanerOnTheWay = 4,
    Arrived = 5,
    InProgress = 6,
    Completed = 7,
    Cancelled = 8
}
