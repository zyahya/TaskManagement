namespace TaskManagement.Domain.Models;

public class TaskItem
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public TaskItemStatus Status { get; set; } = TaskItemStatus.Pending;

    public int UserId { get; set; }

    public User User { get; set; } = null!;
}
