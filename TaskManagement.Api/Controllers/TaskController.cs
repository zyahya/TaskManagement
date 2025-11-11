using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TaskManagement.Core.Dtos;
using TaskManagement.Core.Helpers;
using TaskManagement.Core.Interfaces;

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
    public async Task<IActionResult> Create(TaskItemDto request)
    {
        if (!User.TryGetUserId(out var userId))
            return Unauthorized(new { message = "Invalid token: user id missing." });

        await _taskRepository.CreateAsync(request, userId);

        return Ok(request);
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
    public async Task<IActionResult> GetById(int id)
    {
        if (!User.TryGetUserId(out var userId))
            return Unauthorized(new { message = "Invalid token: user id missing." });

        var item = await _taskRepository.GetByIdAsync(id, userId);
        if (item == null) return NotFound();

        return Ok(item);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, TaskItemDto request)
    {
        if (!User.TryGetUserId(out var userId))
            return Unauthorized(new { message = "Invalid token: user id missing." });

        var updatedTask = await _taskRepository.UpdateAsync(id, userId, request);
        if (updatedTask == null) return NotFound();

        return Ok(updatedTask);
    }

    [Authorize]
    [HttpPatch("{id}")]
    public async Task<IActionResult> PatchUpdate(int id, PatchTaskItemDto request)
    {
        if (!User.TryGetUserId(out var userId))
            return Unauthorized(new { message = "Invalid token: user id missing." });

        var updatedTask = await _taskRepository.PatchUpdateAsync(id, userId, request);
        if (updatedTask == null) return NotFound();

        return Ok(updatedTask);
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
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
