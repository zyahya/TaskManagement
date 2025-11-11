using Microsoft.EntityFrameworkCore;

using TaskManagement.Core.Dtos;
using TaskManagement.Core.Helpers;
using TaskManagement.Core.Interfaces;
using TaskManagement.Core.Models;

namespace TaskManagement.Data.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly AppDbContext _context;

    public TaskRepository(AppDbContext context)
    {
        _context = context;
    }

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

    public async Task<TaskItem?> DeleteAsync(int id, int userId)
    {
        var tasks = await _context.Tasks
            .Where(task => task.UserId == userId)
            .FirstOrDefaultAsync(task => task.Id == id);

        if (tasks == null) return null;

        _context.Remove(tasks);
        await _context.SaveChangesAsync();

        return tasks;
    }

    public async Task DeleteAllAsync(int userId)
    {
        var tasks = await _context.Tasks.Where(task => task.UserId == userId).ToListAsync();
        _context.Tasks.RemoveRange(tasks);
        await _context.SaveChangesAsync();
    }

    public async Task<ICollection<TaskItem>> GetAllAsync(QueryObject query, int userId)
    {
        var queryableTasks = _context.Tasks.AsNoTracking().OrderBy(task => task.Id).Where(task => task.UserId == userId).AsQueryable();

        if (!string.IsNullOrEmpty(query.Title))
        {
            queryableTasks = queryableTasks.Where(item => item.Title.Contains(query.Title));
        }

        if (!string.IsNullOrEmpty(query.Status) && Enum.TryParse<TaskItemStatus>(query.Status, out var status))
        {
            queryableTasks = queryableTasks.Where(item => item.Status == status);
        }

        if (!string.IsNullOrEmpty(query.SortBy))
        {
            switch (query.SortBy.ToLower())
            {
                case "title":
                    queryableTasks = query.IsDescending ? queryableTasks.OrderByDescending(item => item.Title) : queryableTasks.OrderBy(item => item.Title);
                    break;
                case "status":
                    queryableTasks = query.IsDescending ? queryableTasks.OrderByDescending(item => item.Status) : queryableTasks.OrderBy(item => item.Status);
                    break;
                default:
                    queryableTasks = query.IsDescending ? queryableTasks.OrderByDescending(item => item.Id) : queryableTasks.OrderBy(item => item.Id);
                    break;
            }
        }

        if (query.PageNumber < 1) query.PageNumber = 1;
        if (query.PageSize < 1) query.PageSize = 5;

        const int maxPageSize = 50;
        query.PageSize = query.PageSize > maxPageSize ? maxPageSize : query.PageSize;

        var skippedValues = (query.PageNumber - 1) * query.PageSize;
        queryableTasks = queryableTasks.Skip(skippedValues).Take(query.PageSize);

        return await queryableTasks.ToListAsync();
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

        if (request.Title != null) task.Title = request.Title;
        if (request.Description != null) task.Description = request.Description;

        if (request.Status != null) task.Status = (TaskItemStatus)request.Status;

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

        await _context.SaveChangesAsync();

        return task;
    }
}
