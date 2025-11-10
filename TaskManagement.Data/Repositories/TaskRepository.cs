using Microsoft.EntityFrameworkCore;

using TaskManagement.Core.Dtos;
using TaskManagement.Core.Helpers;
using TaskManagement.Core.Interfaces;
using TaskManagement.Core.Models;

namespace TaskManagement.Data.Repositories;

public class TaskRepository(AppDbContext context) : ITaskRepository
{
    private readonly AppDbContext _context = context;

    public async Task<TaskItem> CreateAsync(TaskItemDto request, int userId)
    {
        var taskItem = new TaskItem
        {
            Title = request.Title,
            Description = request.Description,
            Status = request.Status,
            UserId = userId
        };

        await _context.Tasks.AddAsync(taskItem);
        await _context.SaveChangesAsync();

        return taskItem;
    }

    public async Task<TaskItem?> Delete(int id, int userId)
    {
        var tasks = await _context.Tasks
            .Where(task => task.UserId == userId)
            .FirstOrDefaultAsync(task => task.Id == id);

        if (tasks == null) return null;

        _context.Remove(tasks);
        await _context.SaveChangesAsync();

        return tasks;
    }

    public async Task DeleteAll(int userId)
    {
        var tasks = await _context.Tasks.Where(task => task.UserId == userId).ToListAsync();
        _context.Tasks.RemoveRange(tasks);
        await _context.SaveChangesAsync();
    }

    public async Task<ICollection<TaskItem>> GetAllAsync(QueryObject query, int userId)
    {
        var tasks = _context.Tasks.AsNoTracking().Where(task => task.UserId == userId).AsQueryable();

        if (!string.IsNullOrEmpty(query.Title))
        {
            tasks = tasks.Where(item => item.Title.Contains(query.Title));
        }

        if (!string.IsNullOrEmpty(query.Description))
        {
            tasks = tasks.Where(item => item.Description.Contains(query.Description));
        }

        if (!string.IsNullOrEmpty(query.Status) && Enum.TryParse<TaskItemStatus>(query.Status, out var status))
        {
            tasks = tasks.Where(item => item.Status == status);
        }

        if (!string.IsNullOrEmpty(query.SortBy))
        {
            if (query.SortBy.Equals("Title", StringComparison.OrdinalIgnoreCase))
            {
                tasks = query.IsDescending ? tasks.OrderByDescending(item => item.Title) : tasks.OrderBy(item => item.Title);
            }

            if (query.SortBy.Equals("Description", StringComparison.OrdinalIgnoreCase))
            {
                tasks = query.IsDescending ? tasks.OrderByDescending(item => item.Description) : tasks.OrderBy(item => item.Description);
            }

            if (query.SortBy.Equals("Status", StringComparison.OrdinalIgnoreCase))
            {
                tasks = query.IsDescending ? tasks.OrderByDescending(item => item.Status) : tasks.OrderBy(item => item.Status);
            }
        }

        var skippedValues = (query.PageNumber - 1) * query.PageSize;
        tasks = tasks.Skip(skippedValues).Take(query.PageSize);

        return await tasks.ToListAsync();
    }

    public async Task<TaskItem?> GetByIdAsync(int id, int userId)
    {
        var task = await _context.Tasks.AsNoTracking()
            .Where(task => task.UserId == userId)
            .FirstOrDefaultAsync(task => task.Id == id);

        if (task == null) return null;

        return task;
    }

    public async Task<TaskItem?> PatchUpdateAsync(int id, int userId, PatchTaskItemDto request)
    {
        var task = await _context.Tasks
            .Where(task => task.UserId == userId)
            .FirstOrDefaultAsync(task => task.Id == id);

        if (task == null) return null;

        if (task.Title != null) task.Title = request.Title;
        if (task.Description != null) task.Description = request.Description;
        if (task.Status != null) task.Status = (TaskItemStatus)request.Status;

        await _context.SaveChangesAsync();
        return task;
    }

    public async Task<TaskItem?> UpdateAsync(int id, int userId, TaskItemDto request)
    {
        var task = await _context.Tasks
            .Where(task => task.UserId == userId)
            .FirstOrDefaultAsync(task => task.Id == id);

        if (task == null) return null;

        task.Title = request.Title;
        task.Description = request.Description;
        task.Status = request.Status;

        _context.Tasks.Update(task);
        await _context.SaveChangesAsync();

        return task;
    }
}
