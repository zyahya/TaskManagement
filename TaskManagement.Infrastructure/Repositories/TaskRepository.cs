using Microsoft.EntityFrameworkCore;

using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Domain.Helpers;
using TaskManagement.Domain.Models;

namespace TaskManagement.Infrastructure.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly AppDbContext _context;

    public TaskRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<TaskItem> CreateAsync(TaskItem taskItem, int userId)
    {
        taskItem.UserId = userId;
        await _context.Tasks.AddAsync(taskItem);
        await _context.SaveChangesAsync();

        return taskItem;
    }

    public async Task<TaskItem?> DeleteAsync(int id, int userId)
    {
        var task = await GetAsync(id, userId);
        if (task == null) return null;

        _context.Remove(task);
        await _context.SaveChangesAsync();

        return task;
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
        var pageNumber = options.PageNumber < 1 ? 1 : options.PageNumber;
        var pageSize = options.PageSize < 1 ? 5 : options.PageSize;

        const int maxPageSize = 50;
        pageSize = pageSize > maxPageSize ? maxPageSize : pageSize;

        var skippedValues = (pageNumber - 1) * pageSize;

        return query.Skip(skippedValues).Take(pageSize);
    }

    public async Task<TaskItem?> GetAsync(int id, int userId)
    {
        var task = await _context.Tasks
            .Where(task => task.UserId == userId)
            .FirstOrDefaultAsync(task => task.Id == id);

        if (task == null) return null;

        return task;
    }

    public async Task<TaskItem?> UpdateAsync(int id, int userId, TaskItem taskItem)
    {
        var task = await GetAsync(id, userId);
        if (task == null) return null;

        task.Title = taskItem.Title;
        task.Description = taskItem.Description;
        task.Status = taskItem.Status;

        await _context.SaveChangesAsync();

        return task;
    }
}
