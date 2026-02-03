using TaskManagement.Domain.Models;

namespace TaskManagement.Application.Services;

public interface IAuthService
{
    string CreateToken(User user);
    string GenerateRefreshToken();
    bool ValidateRefreshToken(User user, string refreshToken);
}
