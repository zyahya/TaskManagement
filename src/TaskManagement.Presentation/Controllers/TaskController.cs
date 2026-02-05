using Mapster;

using Microsoft.AspNetCore.Authorization;

using TaskManagement.Application.Contracts.TaskItem;
using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Domain.Helpers;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Presentation.Controllers;

[Route("api/[controller]")]
[Authorize]
[ApiController]
public class TaskController : ControllerBase
{
    private readonly ITaskRepository _taskRepository;

    public TaskController(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTaskItemRequest request)
    {
        if (!User.TryGetUserId(out var userId))
            return Unauthorized(new { message = "Invalid token: user id missing." });

        var createdTask = await _taskRepository.CreateAsync(request.Adapt<TaskItem>(), userId);

        return CreatedAtAction(nameof(GetById), new { id = createdTask.Id }, request);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] QueryObject query)
    {
        if (!User.TryGetUserId(out var userId))
            return Unauthorized(new { message = "Invalid token: user id missing." });

        var taskEntities = await _taskRepository.GetAllAsync(query, userId);
        var taskResponses = taskEntities.Select(task => task.Adapt<TaskItemResponse>());

        return Ok(taskResponses);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        if (!User.TryGetUserId(out var userId))
            return Unauthorized(new { message = "Invalid token: user id missing." });

        var task = await _taskRepository.GetAsync(id, userId);
        if (task == null) return NotFound();

        return Ok(task.Adapt<TaskItemResponse>());
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] CreateTaskItemRequest request)
    {
        if (!User.TryGetUserId(out var userId))
            return Unauthorized(new { message = "Invalid token: user id missing." });

        var updatedTask = await _taskRepository.UpdateAsync(id, userId, request.Adapt<TaskItem>());
        if (updatedTask == null) return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        if (!User.TryGetUserId(out var userId))
            return Unauthorized(new { message = "Invalid token: user id missing." });

        var item = await _taskRepository.DeleteAsync(id, userId);
        if (item == null) return NotFound();

        return NoContent();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteAll()
    {
        if (!User.TryGetUserId(out var userId))
            return Unauthorized(new { message = "Invalid token: user id missing." });

        await _taskRepository.DeleteAllAsync(userId);
        return NoContent();
    }
}
