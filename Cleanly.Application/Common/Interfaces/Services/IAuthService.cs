using Cleanly.Application.Common.Models;
using Cleanly.Application.DTOs.Auth;

namespace Cleanly.Application.Common.Interfaces.Services;

public interface IAuthService
{
    Task<Result<AuthUserDto>> RegisterCustomerAsync(RegisterCustomerDto dto, CancellationToken cancellationToken = default);
    Task<Result<AuthUserDto>> RegisterCleanerAsync(RegisterCleanerDto dto, CancellationToken cancellationToken = default);
    Task<Result<AuthUserDto>> LoginAsync(LoginDto dto, CancellationToken cancellationToken = default);
    Task<AuthUserDto?> GetByIdAsync(long userId, CancellationToken cancellationToken = default);
}
