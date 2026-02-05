namespace TaskManagement.Application.Contracts.Authentication;

public class RefreshTokenRequestDto
{
    public int UserId { get; set; }
    public required string RefreshToken { get; set; }
}
