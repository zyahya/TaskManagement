using TaskManagement.Core.Contracts.Request;
using TaskManagement.Core.Helpers;
using Mapster;

namespace TaskManagement.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TaskController : ControllerBase
{
    private readonly ITaskRepository _taskRepository;

    public TaskController(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TaskItemRequest request)
    {
        if (!User.TryGetUserId(out var userId))
            return Unauthorized(new { message = "Invalid token: user id missing." });

        var createdTask = await _taskRepository.CreateAsync(request.Adapt<TaskItem>(), userId);

        return CreatedAtAction(nameof(GetById), new { id = createdTask.Id }, request);
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] QueryObject query)
    {
        if (!User.TryGetUserId(out var userId))
            return Unauthorized(new { message = "Invalid token: user id missing." });

        var items = await _taskRepository.GetAllAsync(query, userId);
        return Ok(items);
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        if (!User.TryGetUserId(out var userId))
            return Unauthorized(new { message = "Invalid token: user id missing." });

        var item = await _taskRepository.GetAsync(id, userId);
        if (item == null) return NotFound();

        return Ok(item);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] TaskItemRequest request)
    {
        if (!User.TryGetUserId(out var userId))
            return Unauthorized(new { message = "Invalid token: user id missing." });

        var updatedTask = await _taskRepository.UpdateAsync(id, userId, request.Adapt<TaskItem>());
        if (updatedTask == null) return NotFound();

        return NoContent();
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        if (!User.TryGetUserId(out var userId))
            return Unauthorized(new { message = "Invalid token: user id missing." });

        var item = await _taskRepository.DeleteAsync(id, userId);
        if (item == null) return NotFound();

        return NoContent();
    }

    [Authorize]
    [HttpDelete]
    public async Task<IActionResult> DeleteAll()
    {
        if (!User.TryGetUserId(out var userId))
            return Unauthorized(new { message = "Invalid token: user id missing." });

        await _taskRepository.DeleteAllAsync(userId);
        return NoContent();
    }
}
