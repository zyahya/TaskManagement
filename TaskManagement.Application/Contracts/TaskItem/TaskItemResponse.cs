using TaskManagement.Domain.Enums;

namespace TaskManagement.Application.Contracts.TaskItem;

public class TaskItemResponse
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public TaskItemStatus Status { get; set; } = TaskItemStatus.Pending;
}
