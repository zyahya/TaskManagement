using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Core.Models;

public class TaskItem
{
    public int Id { get; set; }

    public string Title { get; set; } = default!;

    public string? Description { get; set; }

    [Range(0, 2, ErrorMessage = "Invalid status value.")]
    public TaskItemStatus Status { get; set; } = TaskItemStatus.Pending;

    public int UserId { get; set; }

    public User User { get; set; } = null!;
}
