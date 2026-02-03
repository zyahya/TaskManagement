namespace TaskManagement.Domain.Models;

public class User
{
    public int Id { get; set; }

    public string Username { get; set; } = default!;

    public string PasswordHash { get; set; } = default!;

    public ICollection<TaskItem> TaskItems { get; set; } = [];

    public string? RefreshToken { get; set; }

    public DateTime? RefreshTokenExpiryTime { get; set; }
}
