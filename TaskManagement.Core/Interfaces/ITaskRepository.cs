using TaskManagement.Core.Dtos;
using TaskManagement.Core.Helpers;
using TaskManagement.Core.Models;

namespace TaskManagement.Core.Interfaces;

public interface ITaskRepository
{
    Task<TaskItem> CreateAsync(TaskItemDto request, int UserId);
    Task<ICollection<TaskItem>> GetAllAsync(QueryObject query, int userId);
    Task<TaskItem?> GetByIdAsync(int id, int userId);
    Task<TaskItem?> UpdateAsync(int id, int userId, TaskItemDto request);
    Task<TaskItem?> DeleteAsync(int id, int userId);
    Task DeleteAllAsync(int userId);
    Task<TaskItem?> PatchUpdateAsync(int id, int userId, PatchTaskItemDto request);
}
