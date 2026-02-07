using TaskManagement.Application.Contracts.Authentication;

namespace TaskManagement.Application.Services;

public interface IAuthService
{
    Task<AuthResponse?> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default);
}
