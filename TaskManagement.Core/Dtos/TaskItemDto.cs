using System.ComponentModel.DataAnnotations;

using TaskManagement.Core.Models;

namespace TaskManagement.Core.Dtos;

public class TaskItemDto
{
    [Required, MinLength(1), MaxLength(150)]
    public string? Title { get; set; }

    [MinLength(1), MaxLength(500)]
    public string? Description { get; set; }

    [Range(0, 2, ErrorMessage = "Invalid status value.")]
    public TaskItemStatus Status { get; set; } = TaskItemStatus.Pending;
}
