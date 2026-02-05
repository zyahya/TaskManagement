using TaskManagement.Application.Contracts.Authentication;
using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Application.Services;

namespace TaskManagement.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IAuthService _authService;

    public AuthController(IUserRepository userRepository, IAuthService authService)
    {
        _userRepository = userRepository;
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(UserLoginRequest request)
    {
        var user = await _userRepository.RegisterAsync(request);
        if (user == null)
            return BadRequest("Username is already taken");

        return Ok(user);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(UserLoginRequest request)
    {
        var userToken = await _userRepository.LoginAsync(request);
        if (userToken == null)
            return BadRequest("Invalid username or password");

        return Ok(userToken);
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken(RefreshTokenRequestDto request)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId);
        if (user == null)
            return BadRequest("Invalid user");

        var isValidRefreshToken = _authService.ValidateRefreshToken(user, request.RefreshToken);
        if (!isValidRefreshToken)
            return BadRequest("Invalid refresh token");

        var newAccessToken = _authService.CreateToken(user);
        var newRefreshToken = _authService.GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);

        await _userRepository.UpdateAsync(user);

        return Ok(new TokenResponseDto
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken
        });
    }
}
