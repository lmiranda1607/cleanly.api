using Cleanly.Application.Common.Models;
using Cleanly.Application.DTOs.Requests;

namespace Cleanly.Application.Common.Interfaces.Services;

public interface IRequestService
{
    Task<Result<ServiceRequestDto>> CreateAsync(long customerId, CreateServiceRequestDto dto, CancellationToken cancellationToken = default);
    Task<ServiceRequestDto?> GetByIdAsync(long requestId, long userId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<ServiceRequestDto>> GetMyAsync(long customerId, CancellationToken cancellationToken = default);
    Task<Result<ServiceRequestDto>> CancelAsync(long requestId, long customerId, CancellationToken cancellationToken = default);
    Task<Result<ServiceRequestDto>> AcceptAsync(long requestId, long cleanerId, CancellationToken cancellationToken = default);
    Task<Result<ServiceRequestDto>> ArriveAsync(long requestId, long cleanerId, CancellationToken cancellationToken = default);
    Task<Result<ServiceRequestDto>> StartAsync(long requestId, long cleanerId, CancellationToken cancellationToken = default);
    Task<Result<ServiceRequestDto>> CompleteAsync(long requestId, long cleanerId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<ServiceRequestDto>> GetAvailableForCleanerAsync(CancellationToken cancellationToken = default);
}
