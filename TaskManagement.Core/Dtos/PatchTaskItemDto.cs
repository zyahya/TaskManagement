using System.ComponentModel.DataAnnotations;

using TaskManagement.Core.Models;

namespace TaskManagement.Core.Dtos;

public class PatchTaskItemDto
{
    public string? Title { get; set; }

    public string? Description { get; set; }

    [Range(0, 2, ErrorMessage = "Invalid status value.")]
    public TaskItemStatus? Status { get; set; }
}
