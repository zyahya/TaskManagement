namespace TaskManagement.Core.Contracts.Request;

public record UserLoginRequest(
    string Username,
    string Password
);
