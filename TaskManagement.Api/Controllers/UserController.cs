using Microsoft.AspNetCore.Mvc;

using TaskManagement.Core.Dtos;
using TaskManagement.Core.Interfaces;

namespace TaskManagement.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private IUserRepository _userRepository;

    public UserController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(UserDto userDto)
    {
        var user = await _userRepository.RegisterAsync(userDto);
        if (user == null) return BadRequest("Username is already taken");

        return Ok(user);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(UserDto userDto) // TODO: This should return "token" as a string.
    {
        var user = await _userRepository.LoginAsync(userDto);
        if (user == null) return BadRequest("Invalid username or password");

        return Ok(user);
    }
}
