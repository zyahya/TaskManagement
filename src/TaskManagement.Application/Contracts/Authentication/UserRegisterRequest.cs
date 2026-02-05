namespace TaskManagement.Application.Contracts.Authentication;

public record UserRegisterRequest(
    string Username,
    string Password
);
