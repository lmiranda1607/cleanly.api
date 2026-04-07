using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cleanly.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    [HttpGet("public")]
    [AllowAnonymous]
    public IActionResult Public() => Ok(new { status = "ok", authenticated = false });

    [HttpGet("secure")]
    [Authorize]
    public IActionResult Secure() => Ok(new { status = "ok", authenticated = true });
}
