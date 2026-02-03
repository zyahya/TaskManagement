namespace TaskManagement.Application.Contracts.Authentication;

public record UserLoginRequest(
    string Username,
    string Password
);
