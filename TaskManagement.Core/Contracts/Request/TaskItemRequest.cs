using TaskManagement.Core.Models;

namespace TaskManagement.Core.Contracts.Request;

public class TaskItemRequest
{
    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public TaskItemStatus Status { get; set; } = TaskItemStatus.Pending;
}
