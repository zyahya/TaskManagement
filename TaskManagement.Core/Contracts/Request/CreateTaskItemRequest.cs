namespace TaskManagement.Core.Contracts.Request;

public record CreateTaskItemRequest(
    string Title,
    string Description,
    TaskItemStatus Status
);
