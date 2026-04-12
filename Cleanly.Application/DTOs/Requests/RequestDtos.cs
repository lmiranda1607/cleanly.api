using Cleanly.Domain.Enums;

namespace Cleanly.Application.DTOs.Requests;

public sealed record AddressInputDto(
    string Label,
    string Street,
    string? Number,
    string? Neighborhood,
    string City,
    string State,
    string? ZipCode,
    decimal? Latitude,
    decimal? Longitude);

public sealed record CreateServiceRequestDto(
    AddressInputDto Address,
    DateTimeOffset RequestedDate,
    int DurationHours,
    string? Notes);

public sealed record ServiceRequestDto(
    long Id,
    long CustomerId,
    long AddressId,
    DateTimeOffset RequestedDate,
    int DurationHours,
    string? Notes,
    ServiceRequestStatus Status,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt,
    long? AssignedCleanerId,
    DateTimeOffset? AcceptedAt,
    DateTimeOffset? StartedAt,
    DateTimeOffset? FinishedAt);
