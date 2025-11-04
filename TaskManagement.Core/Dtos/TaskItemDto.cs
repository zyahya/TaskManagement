using System.ComponentModel.DataAnnotations;

using TaskManagement.Core.Models;

namespace TaskManagement.Core.Dtos;

public class TaskItemDto
{
    [Required, MinLength(1), MaxLength(150)]
    public string? Title { get; set; }

    [MinLength(1), MaxLength(500)]
    public string? Description { get; set; }

    [Required]
    public TaskItemStatus Status { get; set; } = TaskItemStatus.Pending;
}
