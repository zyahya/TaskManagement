using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Core.Models;

public class TaskItem
{
    [Key]
    public int Id { get; set; }

    [Required, MinLength(1), MaxLength(150)]
    public string Title { get; set; } = default!;

    [MinLength(1), MaxLength(500)]
    public string? Description { get; set; }

    [Range(0, 2, ErrorMessage = "Invalid status value.")]
    public TaskItemStatus Status { get; set; } = TaskItemStatus.Pending;

    public int UserId { get; set; }

    public User User { get; set; } = null!;
}
