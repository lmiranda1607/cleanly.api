using Cleanly.Application.Common.Interfaces;
using Cleanly.Application.Common.Interfaces.Services;
using Cleanly.Application.Common.Models;
using Cleanly.Application.DTOs.Ratings;
using Cleanly.Domain.Entities;
using Cleanly.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Cleanly.Application.Services;

public sealed class RatingService(IApplicationDbContext dbContext) : IRatingService
{
    public async Task<Result<RatingDto>> CreateAsync(long requestId, long customerId, CreateRatingDto dto, CancellationToken cancellationToken = default)
    {
        if (dto.Score < 1 || dto.Score > 5)
        {
            return Result<RatingDto>.Fail("Score must be between 1 and 5.");
        }

        var request = await dbContext.ServiceRequests.Include(x => x.Match)
            .Include(x => x.Rating)
            .FirstOrDefaultAsync(x => x.Id == requestId && x.CustomerId == customerId, cancellationToken);

        if (request is null)
        {
            return Result<RatingDto>.Fail("Request not found.");
        }

        if (request.Status != ServiceRequestStatus.Completed)
        {
            return Result<RatingDto>.Fail("Request must be completed before rating.");
        }

        if (request.Rating is not null)
        {
            return Result<RatingDto>.Fail("Request already rated.");
        }

        if (request.Match is null)
        {
            return Result<RatingDto>.Fail("No cleaner assigned for this request.");
        }

        var rating = new Rating(request.Id, customerId, request.Match.CleanerId, dto.Score, dto.Comment);
        dbContext.Ratings.Add(rating);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result<RatingDto>.Ok(Map(rating));
    }

    public async Task<IReadOnlyCollection<RatingDto>> GetByCleanerAsync(long cleanerId, CancellationToken cancellationToken = default)
    {
        var ratings = await dbContext.Ratings.AsNoTracking()
            .Where(x => x.CleanerId == cleanerId)
            .OrderByDescending(x => x.CreatedAt)
            .Take(100)
            .ToListAsync(cancellationToken);

        return ratings.Select(Map).ToArray();
    }

    private static RatingDto Map(Rating rating)
        => new(rating.Id, rating.RequestId, rating.CustomerId, rating.CleanerId, rating.Score, rating.Comment, rating.CreatedAt);
}
