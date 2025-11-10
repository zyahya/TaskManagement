using Microsoft.AspNetCore.Mvc;

using TaskManagement.Core.Dtos;
using TaskManagement.Core.Interfaces;
using TaskManagement.Core.Models;

namespace TaskManagement.Api.Controllers;

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
    public async Task<IActionResult> Register(UserDto userDto)
    {
        var user = await _userRepository.RegisterAsync(userDto);
        if (user == null) return BadRequest("Username is already taken");

        return Ok(user);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(UserDto userDto)
    {
        var userToken = await _userRepository.LoginAsync(userDto);
        if (userToken == null) return BadRequest("Invalid username or password");

        return Ok(userToken);
    }

    [HttpGet("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromQuery] int userId, [FromQuery] string refreshToken)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null) return BadRequest("Invalid user");

        var isValidRefreshToken = _authService.ValidateRefreshToken(user, refreshToken);
        if (!isValidRefreshToken) return BadRequest("Invalid refresh token");

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
