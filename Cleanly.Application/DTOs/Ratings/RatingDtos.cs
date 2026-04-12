namespace Cleanly.Application.DTOs.Ratings;

public sealed record CreateRatingDto(int Score, string? Comment);

public sealed record RatingDto(long Id, long RequestId, long CustomerId, long CleanerId, int Score, string? Comment, DateTimeOffset CreatedAt);
