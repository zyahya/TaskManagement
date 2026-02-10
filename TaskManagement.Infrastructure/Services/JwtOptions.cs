using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Infrastructure.Services;

public class JwtOptions
{
    public static string SectionName = "Jwt";

    [Required]
    public string Key { get; set; } = string.Empty;

    [Required]
    public string Issuer { get; set; } = string.Empty;

    [Required]
    public string Audience { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "Expiry minutes must be greater than 1")]
    public int ExpiryMinutes { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Expiry days must be greater than 1")]
    public int RefreshTokenExpiryDays { get; set; }
}
