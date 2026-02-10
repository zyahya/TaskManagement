using TaskManagement.Application.Contracts.Authentication;
using TaskManagement.Application.Services;

namespace TaskManagement.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] AuthRequest request)
    {
        var result = await _authService.GetTokenAsync(request.Email, request.Password);

        if (result is null)
            return BadRequest();

        return Ok(result);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshAsync([FromBody] RefreshTokenRequest request)
    {
        var result = await _authService.GetRefreshTokenAsync(request.Token, request.RefreshToken);

        return result is null ? BadRequest("Invalid token.") : Ok(result);
    }

    [HttpPost("revoke-refresh-token")]
    public async Task<IActionResult> RevokeRefreshTokenAsync([FromBody] RefreshTokenRequest request)
    {
        var isRevoked = await _authService.RevokeRefreshTokenAsync(request.Token, request.RefreshToken);

        return isRevoked ? Ok() : BadRequest("Operation failed.");
    }
}
