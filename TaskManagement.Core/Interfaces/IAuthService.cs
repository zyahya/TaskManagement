using TaskManagement.Core.Models;

namespace TaskManagement.Core.Interfaces;

public interface IAuthService
{
    string CreateToken(User user);
    string GenerateRefreshToken();
    bool ValidateRefreshToken(User user, string refreshToken);
}
