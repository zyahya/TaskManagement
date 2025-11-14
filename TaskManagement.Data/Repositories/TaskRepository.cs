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

        queryableTasks = ApplyFilters(queryableTasks, query);
        queryableTasks = ApplySorting(queryableTasks, query);
        queryableTasks = ApplyPagination(queryableTasks, query);

        return await queryableTasks.ToListAsync();
    }

    private static IQueryable<TaskItem> ApplyFilters(IQueryable<TaskItem> query, QueryObject options)
    {
        if (!string.IsNullOrEmpty(options.Title))
        {
            query = query.Where(item => item.Title.Contains(options.Title));
        }

        if (!string.IsNullOrEmpty(options.Status) && Enum.TryParse<TaskItemStatus>(options.Status, out var status))
        {
            query = query.Where(item => item.Status == status);
        }

        return query;
    }

    private static IQueryable<TaskItem> ApplySorting(IQueryable<TaskItem> query, QueryObject options)
    {
        if (!string.IsNullOrEmpty(options.SortBy))
        {
            query = options.SortBy?.ToLowerInvariant() switch
            {
                "title" => options.IsDescending ? query.OrderByDescending(item => item.Title) : query.OrderBy(item => item.Title),
                "status" => options.IsDescending ? query.OrderByDescending(item => item.Status) : query.OrderBy(item => item.Status),
                _ => options.IsDescending ? query.OrderByDescending(item => item.Id) : query.OrderBy(item => item.Id),
            };
        }

        return query;
    }

    private static IQueryable<TaskItem> ApplyPagination(IQueryable<TaskItem> query, QueryObject options)
    {
        int PageNumber = options.PageNumber < 1 ? 1 : options.PageNumber;
        int PageSize = options.PageSize < 1 ? 5 : options.PageSize;

        const int maxPageSize = 50;
        PageSize = PageSize > maxPageSize ? maxPageSize : PageSize;

        var skippedValues = (PageNumber - 1) * PageSize;

        return query.Skip(skippedValues).Take(PageSize);
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
