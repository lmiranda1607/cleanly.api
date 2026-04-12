namespace Cleanly.Application.DTOs.Cleaners;

public sealed record UpdateCleanerLocationDto(decimal Latitude, decimal Longitude);

public sealed record CleanerStatusDto(long CleanerId, bool Active);

public sealed record CleanerLocationDto(long CleanerId, decimal Latitude, decimal Longitude, DateTimeOffset UpdatedAt);
