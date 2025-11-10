using Microsoft.AspNetCore.Mvc;

namespace TaskManagement.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TestController : ControllerBase
{
    [HttpGet]
    public string Test()
    {
        return "Hello, World";
    }
}
