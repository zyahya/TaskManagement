using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Core.Dtos;

public class UserLoginDto
{
    [Required, MinLength(3), MaxLength(14)]
    public string Username { get; set; } = default!;

    [Required, MinLength(8), MaxLength(50)]
    public string Password { get; set; } = default!;
}
