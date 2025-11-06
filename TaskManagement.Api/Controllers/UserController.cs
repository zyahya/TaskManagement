using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using TaskManagement.Core.Dtos;
using TaskManagement.Core.Interfaces;
using TaskManagement.Core.Models;
using TaskManagement.Data.Repositories;

namespace TaskManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserDto userDto)
        {
            var user = await _userRepository.Register(userDto);
            return Ok(user);
        }
    }
}
