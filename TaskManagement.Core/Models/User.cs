using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Core.Models;

public class User
{
    [Key]
    public int Id { get; set; }

    [Required, MinLength(3), MaxLength(14)]
    public string Username { get; set; } = default!;

    [Required, MinLength(1), MaxLength(256)]
    public string PasswordHash { get; set; } = default!;

    public ICollection<TaskItem> Tasks { get; set; } = [];

    public string? RefreshToken { get; set; }

    public DateTime? RefreshTokenExpiryTime { get; set; }
}
