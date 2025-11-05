using TaskManagement.Core.Dtos;
using TaskManagement.Core.Helpers;
using TaskManagement.Core.Models;

namespace TaskManagement.Core.Interfaces;

public interface ITaskRepository
{
    Task<TaskItem> CreateAsync(TaskItemDto taskItemDto);
    Task<ICollection<TaskItem>> GetAllAsync(QueryObject query);
    Task<TaskItem?> GetByIdAsync(int id);
    Task<TaskItem?> UpdateAsync(int id, TaskItemDto taskItemDto);
    Task<TaskItem?> Delete(int id);
    Task DeleteAll();
    Task PatchUpdateAsync(TaskItem item);
}
