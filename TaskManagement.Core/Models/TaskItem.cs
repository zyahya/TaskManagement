using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Core.Models;

public class TaskItem
{
    public int Id { get; set; }

    public string Title { get; set; } = default!;

    public string? Description { get; set; }

    /* TODO:
        Replace this validation to 'FluentValidation'
        Check: https://gemini.google.com/app/a5e94dc5ba5ecf7f
    */
    [Range(0, 2, ErrorMessage = "Invalid status value.")]
    // This default value is at the object level
    // Default value set here for the application layer
    public TaskItemStatus Status { get; set; } = TaskItemStatus.Pending;

    public int UserId { get; set; }

    public User User { get; set; } = null!;
}
