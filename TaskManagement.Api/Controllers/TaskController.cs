using Microsoft.AspNetCore.Mvc;

using TaskManagement.Core.Dtos;
using TaskManagement.Core.Interfaces;
using TaskManagement.Core.Models;

namespace TaskManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private ITaskRepository _taskRepository;

        public TaskController(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Create(TaskItemDto taskItem)
        {
            await _taskRepository.CreateAsync(taskItem);
            return Ok(taskItem);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _taskRepository.GetAllAsync();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _taskRepository.GetByIdAsync(id);
            if (item == null) return NotFound();

            return Ok(item);
        }

        [HttpPut]
        public async Task<IActionResult> Update(int id, TaskItemDto request)
        {
            var item = await _taskRepository.UpdateAsync(id, request);
            if (item == null) return NotFound();

            return Ok(item);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchUpdate(int id, PatchTaskItemDto request)
        {
            var item = await _taskRepository.GetByIdAsync(id);
            if (item == null) return NotFound();

            if (request.Title != null) item.Title = request.Title;
            if (request.Description != null) item.Description = request.Description;
            if (request.Status != null) item.Status = (TaskItemStatus)request.Status;

            await _taskRepository.PatchUpdateAsync(item);
            return Ok(item);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _taskRepository.Delete(id);
            if (item == null) return NotFound();

            return NoContent();
        }
    }
}
