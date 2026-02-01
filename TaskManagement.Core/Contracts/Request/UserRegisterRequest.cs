namespace TaskManagement.Core.Contracts.Request;

public record UserRegisterRequest(
    string Username,
    string Password
);
