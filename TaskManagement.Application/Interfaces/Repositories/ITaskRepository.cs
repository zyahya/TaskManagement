using TaskManagement.Domain.Helpers;
using TaskManagement.Domain.Models;

namespace TaskManagement.Application.Interfaces.Repositories;

public interface ITaskRepository
{
    Task<TaskItem> CreateAsync(TaskItem taskItem, int userId);

    Task<ICollection<TaskItem>> GetAllAsync(QueryObject query, int userId);

    Task<TaskItem?> GetAsync(int id, int userId);

    Task<TaskItem?> UpdateAsync(int id, int userId, TaskItem taskItem);

    Task<TaskItem?> DeleteAsync(int id, int userId);

    Task DeleteAllAsync(int userId);
}
