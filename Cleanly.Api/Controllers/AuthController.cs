using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Cleanly.Api.Common;
using Cleanly.Api.Services;
using Cleanly.Application.Common.Interfaces.Services;
using Cleanly.Application.DTOs.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cleanly.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IAuthService authService, JwtTokenService jwtTokenService) : ControllerBase
{
    [HttpPost("register/customer")]
    [AllowAnonymous]
    public async Task<IActionResult> RegisterCustomer([FromBody] RegisterCustomerDto dto, CancellationToken cancellationToken)
    {
        var result = await authService.RegisterCustomerAsync(dto, cancellationToken);
        if (!result.Success || result.Value is null)
        {
            return BadRequest(ApiResponse<object>.Fail(result.Error ?? "Unable to register customer."));
        }

        return Ok(ApiResponse<object>.Ok(new
        {
            user = result.Value,
            token = jwtTokenService.CreateToken(result.Value)
        }));
    }

    [HttpPost("register/cleaner")]
    [AllowAnonymous]
    public async Task<IActionResult> RegisterCleaner([FromBody] RegisterCleanerDto dto, CancellationToken cancellationToken)
    {
        var result = await authService.RegisterCleanerAsync(dto, cancellationToken);
        if (!result.Success || result.Value is null)
        {
            return BadRequest(ApiResponse<object>.Fail(result.Error ?? "Unable to register cleaner."));
        }

        return Ok(ApiResponse<object>.Ok(new
        {
            user = result.Value,
            token = jwtTokenService.CreateToken(result.Value)
        }));
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginDto dto, CancellationToken cancellationToken)
    {
        var result = await authService.LoginAsync(dto, cancellationToken);
        if (!result.Success || result.Value is null)
        {
            return Unauthorized(ApiResponse<object>.Fail(result.Error ?? "Invalid credentials."));
        }

        return Ok(ApiResponse<object>.Ok(new
        {
            user = result.Value,
            token = jwtTokenService.CreateToken(result.Value)
        }));
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> Me(CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        if (userId is null)
        {
            return Unauthorized(ApiResponse<object>.Fail("Invalid token."));
        }

        var user = await authService.GetByIdAsync(userId.Value, cancellationToken);
        if (user is null)
        {
            return NotFound(ApiResponse<object>.Fail("User not found."));
        }

        return Ok(ApiResponse<object>.Ok(user));
    }

    private long? GetUserId()
    {
        var raw = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);
        return long.TryParse(raw, out var id) ? id : null;
    }
}
