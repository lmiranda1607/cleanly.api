using Cleanly.Application.Common.Models;
using Cleanly.Application.DTOs.Cleaners;

namespace Cleanly.Application.Common.Interfaces.Services;

public interface ICleanerService
{
    Task<Result<CleanerStatusDto>> SetOnlineAsync(long cleanerId, CancellationToken cancellationToken = default);
    Task<Result<CleanerStatusDto>> SetOfflineAsync(long cleanerId, CancellationToken cancellationToken = default);
    Task<Result<CleanerLocationDto>> UpdateLocationAsync(long cleanerId, UpdateCleanerLocationDto dto, CancellationToken cancellationToken = default);
}
