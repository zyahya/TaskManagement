using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Core.Models;

public class User
{
    [Key]
    public int Id { get; set; }

    [Required, MinLength(3), MaxLength(14)]
    public string Username { get; set; }

    [Required, MinLength(1), MaxLength(256)]
    public string PasswordHash { get; set; }

    public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
}
