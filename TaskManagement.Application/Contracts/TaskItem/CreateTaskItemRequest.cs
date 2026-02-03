using TaskManagement.Domain.Models;

namespace TaskManagement.Application.Contracts.TaskItem;

public record CreateTaskItemRequest(
    string Title,
    string Description,
    TaskItemStatus Status
);
