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
}
