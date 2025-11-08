using Microsoft.AspNetCore.Mvc;

using TaskManagement.Core.Dtos;
using TaskManagement.Core.Interfaces;

namespace TaskManagement.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IUserRepository userRepository) : ControllerBase
{
    private readonly IUserRepository _userRepository = userRepository;

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
}
