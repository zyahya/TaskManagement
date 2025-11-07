using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Core.Dtos;

public class UserDto
{
    [Required, MinLength(3), MaxLength(14)]
    public string Username { get; set; }

    // [Required, MinLength(3), MaxLength(20)]
    // public string? FirstName { get; set; }

    // [Required, MinLength(1), MaxLength(20)]
    // public string? LastName { get; set; }

    // [Required, EmailAddress]
    // public string? Email { get; set; }

    [Required, MinLength(8), MaxLength(50)]
    public string Password { get; set; }
}
