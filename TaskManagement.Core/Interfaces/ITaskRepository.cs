using TaskManagement.Core.Dtos;
using TaskManagement.Core.Helpers;
using TaskManagement.Core.Models;

namespace TaskManagement.Core.Interfaces;

public interface ITaskRepository
{
    /// <summary>
    /// Persists a new task that belongs to the specified user using the provided payload.
    /// </summary>
    /// <param name="request">Payload containing title, description, and status for the new task.</param>
    /// <param name="UserId">Identifier of the user that will own the created task.</param>
    /// <returns>The newly created <see cref="TaskItem"/>.</returns>
    Task<TaskItem> CreateAsync(TaskItemDto request, int UserId);

    /// <summary>
    /// Retrieves all tasks for a user, applying filtering, sorting, and pagination rules from the query object.
    /// </summary>
    /// <param name="query">Filtering, sorting, and paging options.</param>
    /// <param name="userId">Identifier of the user whose tasks should be returned.</param>
    /// <returns>A collection of matching <see cref="TaskItem"/> records.</returns>
    Task<ICollection<TaskItem>> GetAllAsync(QueryObject query, int userId);

    /// <summary>
    /// Fetches a single task that belongs to the specified user by its identifier.
    /// </summary>
    /// <param name="id">Identifier of the task to retrieve.</param>
    /// <param name="userId">Identifier of the user that owns the task.</param>
    /// <returns>The matching <see cref="TaskItem"/> if found; otherwise <c>null</c>.</returns>
    Task<TaskItem?> GetByIdAsync(int id, int userId);

    /// <summary>
    /// Replaces the entire task payload for the specified user with the provided values.
    /// </summary>
    /// <param name="id">Identifier of the task to update.</param>
    /// <param name="userId">Identifier of the user that owns the task.</param>
    /// <param name="request">Replacement payload containing the new title, description, and status.</param>
    /// <returns>The updated <see cref="TaskItem"/> if the record exists; otherwise <c>null</c>.</returns>
    Task<TaskItem?> UpdateAsync(int id, int userId, TaskItemDto request);

    /// <summary>
    /// Deletes a single task that belongs to the specified user.
    /// </summary>
    /// <param name="id">Identifier of the task to remove.</param>
    /// <param name="userId">Identifier of the user that owns the task.</param>
    /// <returns>The deleted <see cref="TaskItem"/> if the record existed; otherwise <c>null</c>.</returns>
    Task<TaskItem?> DeleteAsync(int id, int userId);

    /// <summary>
    /// Removes every task associated with the specified user.
    /// </summary>
    /// <param name="userId">Identifier of the user whose tasks should be deleted.</param>
    Task DeleteAllAsync(int userId);

    /// <summary>
    /// Applies partial updates to a task for the specified user without replacing unspecified fields.
    /// </summary>
    /// <param name="id">Identifier of the task to modify.</param>
    /// <param name="userId">Identifier of the user that owns the task.</param>
    /// <param name="request">Payload with optional fields to update.</param>
    /// <returns>The patched <see cref="TaskItem"/> if the record exists; otherwise <c>null</c>.</returns>
    Task<TaskItem?> PatchUpdateAsync(int id, int userId, PatchTaskItemDto request);
}
