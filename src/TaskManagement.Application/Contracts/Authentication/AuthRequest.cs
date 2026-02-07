namespace TaskManagement.Application.Contracts.Authentication;

public record AuthRequest(
    string Email,
    string Password
);
