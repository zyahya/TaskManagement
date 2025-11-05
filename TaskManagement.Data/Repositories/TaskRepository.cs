using Microsoft.EntityFrameworkCore;

using TaskManagement.Core.Dtos;
using TaskManagement.Core.Helpers;
using TaskManagement.Core.Interfaces;
using TaskManagement.Core.Models;

namespace TaskManagement.Data.Repositories;

public class TaskRepository : ITaskRepository
{
    private AppDbContext _context;

    public TaskRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<TaskItem> CreateAsync(TaskItemDto taskItemDto)
    {
        var taskItem = new TaskItem
        {
            Title = taskItemDto.Title,
            Description = taskItemDto.Description,
            Status = taskItemDto.Status
        };
        await _context.Tasks.AddAsync(taskItem);
        await _context.SaveChangesAsync();

        return taskItem;
    }

    public async Task<TaskItem?> Delete(int id)
    {
        var item = await _context.Tasks.FindAsync(id);
        if (item == null) return null;

        _context.Remove(item);
        await _context.SaveChangesAsync();

        return item;
    }

    public async Task<ICollection<TaskItem>> GetAllAsync(QueryObject query)
    {
        var items = _context.Tasks.AsQueryable();

        if (!string.IsNullOrEmpty(query.Title))
        {
            items = items.Where(item => item.Title.Contains(query.Title));
        }

        if (!string.IsNullOrEmpty(query.Description))
        {
            items = items.Where(item => item.Description.Contains(query.Description));
        }

        if (!string.IsNullOrEmpty(query.Status) && Enum.TryParse<TaskItemStatus>(query.Status, out var status))
        {
            items = items.Where(item => item.Status == status);
        }

        var skippedValues = (query.PageNumber - 1) * query.PageSize;
        items = items.Skip(skippedValues).Take(query.PageSize);

        return await items.ToListAsync();
    }

    public async Task<TaskItem?> GetByIdAsync(int id)
    {
        var item = await _context.Tasks.FindAsync(id);
        if (item == null) return null;

        return item;
    }

    public async Task PatchUpdateAsync(TaskItem item)
    {
        _context.Tasks.Update(item);
        await _context.SaveChangesAsync();
    }

    public async Task<TaskItem?> UpdateAsync(int id, TaskItemDto taskItemDto)
    {
        var item = await _context.Tasks.FindAsync(id);
        if (item == null) return null;

        item.Title = taskItemDto.Title;
        item.Description = taskItemDto.Description;
        item.Status = taskItemDto.Status;

        _context.Tasks.Update(item);
        await _context.SaveChangesAsync();

        return item;
    }
}
