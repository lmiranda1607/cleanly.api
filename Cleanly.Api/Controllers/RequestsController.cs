using System.Security.Claims;
using Cleanly.Api.Common;
using Cleanly.Application.Common.Interfaces.Services;
using Cleanly.Application.DTOs.Ratings;
using Cleanly.Application.DTOs.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cleanly.Api.Controllers;

[ApiController]
[Route("api/requests")]
[Authorize]
public class RequestsController(IRequestService requestService, IRatingService ratingService) : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> Create([FromBody] CreateServiceRequestDto dto, CancellationToken cancellationToken)
    {
        var customerId = GetUserId();
        if (customerId is null)
        {
            return Unauthorized(ApiResponse<object>.Fail("Invalid token."));
        }

        var result = await requestService.CreateAsync(customerId.Value, dto, cancellationToken);
        return result.Success && result.Value is not null
            ? Ok(ApiResponse<object>.Ok(result.Value))
            : BadRequest(ApiResponse<object>.Fail(result.Error ?? "Unable to create request."));
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        if (userId is null)
        {
            return Unauthorized(ApiResponse<object>.Fail("Invalid token."));
        }

        var request = await requestService.GetByIdAsync(id, userId.Value, cancellationToken);
        return request is null
            ? NotFound(ApiResponse<object>.Fail("Request not found."))
            : Ok(ApiResponse<object>.Ok(request));
    }

    [HttpGet("my")]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> My(CancellationToken cancellationToken)
    {
        var customerId = GetUserId();
        if (customerId is null)
        {
            return Unauthorized(ApiResponse<object>.Fail("Invalid token."));
        }

        var requests = await requestService.GetMyAsync(customerId.Value, cancellationToken);
        return Ok(ApiResponse<object>.Ok(requests));
    }

    [HttpPost("{id:long}/cancel")]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> Cancel(long id, CancellationToken cancellationToken)
    {
        var customerId = GetUserId();
        if (customerId is null)
        {
            return Unauthorized(ApiResponse<object>.Fail("Invalid token."));
        }

        var result = await requestService.CancelAsync(id, customerId.Value, cancellationToken);
        return result.Success && result.Value is not null
            ? Ok(ApiResponse<object>.Ok(result.Value))
            : BadRequest(ApiResponse<object>.Fail(result.Error ?? "Unable to cancel request."));
    }

    [HttpPost("{id:long}/accept")]
    [Authorize(Roles = "Cleaner")]
    public async Task<IActionResult> Accept(long id, CancellationToken cancellationToken)
    {
        var cleanerId = GetUserId();
        if (cleanerId is null)
        {
            return Unauthorized(ApiResponse<object>.Fail("Invalid token."));
        }

        var result = await requestService.AcceptAsync(id, cleanerId.Value, cancellationToken);
        return result.Success && result.Value is not null
            ? Ok(ApiResponse<object>.Ok(result.Value))
            : BadRequest(ApiResponse<object>.Fail(result.Error ?? "Unable to accept request."));
    }

    [HttpPost("{id:long}/arrive")]
    [Authorize(Roles = "Cleaner")]
    public async Task<IActionResult> Arrive(long id, CancellationToken cancellationToken)
    {
        var cleanerId = GetUserId();
        if (cleanerId is null)
        {
            return Unauthorized(ApiResponse<object>.Fail("Invalid token."));
        }

        var result = await requestService.ArriveAsync(id, cleanerId.Value, cancellationToken);
        return result.Success && result.Value is not null
            ? Ok(ApiResponse<object>.Ok(result.Value))
            : BadRequest(ApiResponse<object>.Fail(result.Error ?? "Unable to set arrival."));
    }

    [HttpPost("{id:long}/start")]
    [Authorize(Roles = "Cleaner")]
    public async Task<IActionResult> Start(long id, CancellationToken cancellationToken)
    {
        var cleanerId = GetUserId();
        if (cleanerId is null)
        {
            return Unauthorized(ApiResponse<object>.Fail("Invalid token."));
        }

        var result = await requestService.StartAsync(id, cleanerId.Value, cancellationToken);
        return result.Success && result.Value is not null
            ? Ok(ApiResponse<object>.Ok(result.Value))
            : BadRequest(ApiResponse<object>.Fail(result.Error ?? "Unable to start request."));
    }

    [HttpPost("{id:long}/complete")]
    [Authorize(Roles = "Cleaner")]
    public async Task<IActionResult> Complete(long id, CancellationToken cancellationToken)
    {
        var cleanerId = GetUserId();
        if (cleanerId is null)
        {
            return Unauthorized(ApiResponse<object>.Fail("Invalid token."));
        }

        var result = await requestService.CompleteAsync(id, cleanerId.Value, cancellationToken);
        return result.Success && result.Value is not null
            ? Ok(ApiResponse<object>.Ok(result.Value))
            : BadRequest(ApiResponse<object>.Fail(result.Error ?? "Unable to complete request."));
    }

    [HttpPost("{id:long}/rating")]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> Rate(long id, [FromBody] CreateRatingDto dto, CancellationToken cancellationToken)
    {
        var customerId = GetUserId();
        if (customerId is null)
        {
            return Unauthorized(ApiResponse<object>.Fail("Invalid token."));
        }

        var result = await ratingService.CreateAsync(id, customerId.Value, dto, cancellationToken);
        return result.Success && result.Value is not null
            ? Ok(ApiResponse<object>.Ok(result.Value))
            : BadRequest(ApiResponse<object>.Fail(result.Error ?? "Unable to create rating."));
    }

    private long? GetUserId()
    {
        var raw = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return long.TryParse(raw, out var id) ? id : null;
    }
}
