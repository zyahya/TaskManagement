using TaskManagement.Domain.Enums;

namespace TaskManagement.Application.Contracts.TaskItem;

public record CreateTaskItemRequest(
    string Title,
    string Description,
    TaskItemStatus Status
);
