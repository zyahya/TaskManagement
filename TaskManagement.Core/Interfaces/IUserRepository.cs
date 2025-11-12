using TaskManagement.Core.Dtos;
using TaskManagement.Core.Models;

namespace TaskManagement.Core.Interfaces;

public interface IUserRepository
{
    Task<User?> RegisterAsync(UserLoginDto request);
    Task<TokenResponseDto?> LoginAsync(UserLoginDto request);
    Task<User?> GetByIdAsync(int id);
    Task UpdateAsync(User user);
}
