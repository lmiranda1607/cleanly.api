using System.Security.Claims;
using Cleanly.Api.Common;
using Cleanly.Application.Common.Interfaces.Services;
using Cleanly.Application.DTOs.Cleaners;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cleanly.Api.Controllers;

[ApiController]
[Route("api/cleaners")]
public class CleanersController(ICleanerService cleanerService, IRequestService requestService, IRatingService ratingService) : ControllerBase
{
    [HttpPost("me/online")]
    [Authorize(Roles = "Cleaner")]
    public async Task<IActionResult> Online(CancellationToken cancellationToken)
    {
        var cleanerId = GetUserId();
        if (cleanerId is null)
        {
            return Unauthorized(ApiResponse<object>.Fail("Invalid token."));
        }

        var result = await cleanerService.SetOnlineAsync(cleanerId.Value, cancellationToken);
        return result.Success && result.Value is not null
            ? Ok(ApiResponse<object>.Ok(result.Value))
            : BadRequest(ApiResponse<object>.Fail(result.Error ?? "Unable to set online."));
    }

    [HttpPost("me/offline")]
    [Authorize(Roles = "Cleaner")]
    public async Task<IActionResult> Offline(CancellationToken cancellationToken)
    {
        var cleanerId = GetUserId();
        if (cleanerId is null)
        {
            return Unauthorized(ApiResponse<object>.Fail("Invalid token."));
        }

        var result = await cleanerService.SetOfflineAsync(cleanerId.Value, cancellationToken);
        return result.Success && result.Value is not null
            ? Ok(ApiResponse<object>.Ok(result.Value))
            : BadRequest(ApiResponse<object>.Fail(result.Error ?? "Unable to set offline."));
    }

    [HttpPost("me/location")]
    [Authorize(Roles = "Cleaner")]
    public async Task<IActionResult> Location([FromBody] UpdateCleanerLocationDto dto, CancellationToken cancellationToken)
    {
        var cleanerId = GetUserId();
        if (cleanerId is null)
        {
            return Unauthorized(ApiResponse<object>.Fail("Invalid token."));
        }

        var result = await cleanerService.UpdateLocationAsync(cleanerId.Value, dto, cancellationToken);
        return result.Success && result.Value is not null
            ? Ok(ApiResponse<object>.Ok(result.Value))
            : BadRequest(ApiResponse<object>.Fail(result.Error ?? "Unable to update location."));
    }

    [HttpGet("me/requests/available")]
    [Authorize(Roles = "Cleaner")]
    public async Task<IActionResult> AvailableRequests(CancellationToken cancellationToken)
    {
        var requests = await requestService.GetAvailableForCleanerAsync(cancellationToken);
        return Ok(ApiResponse<object>.Ok(requests));
    }

    [HttpGet("{id:long}/ratings")]
    [AllowAnonymous]
    public async Task<IActionResult> Ratings(long id, CancellationToken cancellationToken)
    {
        var ratings = await ratingService.GetByCleanerAsync(id, cancellationToken);
        return Ok(ApiResponse<object>.Ok(ratings));
    }

    private long? GetUserId()
    {
        var raw = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return long.TryParse(raw, out var id) ? id : null;
    }
}
