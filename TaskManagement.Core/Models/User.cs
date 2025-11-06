using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Core.Models;

public class User
{
    [Key]
    public int Id { get; set; }

    [Required, MinLength(3), MaxLength(14)]
    public string? Username { get; set; }

    [Required, MinLength(3), MaxLength(20)]
    public string? FirstName { get; set; }

    [Required, MinLength(1), MaxLength(20)]
    public string? LastName { get; set; }

    [Required, EmailAddress]
    public string? Email { get; set; }

    [Required, MinLength(1), MaxLength(256)]
    public string PasswordHash { get; set; }
}
