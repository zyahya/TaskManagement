namespace TaskManagement.Core.Models;

public class TaskItem
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public DateTime DueDate { get; set; }
    public TaskStatus Status { get; set; } = TaskStatus.Pending;
}
