using TaskManagement.Core.Models;

namespace TaskManagement.Core.Interfaces;

public interface IJwtService
{
    string CreateToken(User user);
}
