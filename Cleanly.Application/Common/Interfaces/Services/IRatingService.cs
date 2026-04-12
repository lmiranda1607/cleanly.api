using Cleanly.Application.Common.Models;
using Cleanly.Application.DTOs.Ratings;

namespace Cleanly.Application.Common.Interfaces.Services;

public interface IRatingService
{
    Task<Result<RatingDto>> CreateAsync(long requestId, long customerId, CreateRatingDto dto, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<RatingDto>> GetByCleanerAsync(long cleanerId, CancellationToken cancellationToken = default);
}
