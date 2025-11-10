using TaskManagement.Core.Dtos;
using TaskManagement.Core.Models;

namespace TaskManagement.Core.Interfaces;

public interface IUserRepository
{
    Task<User?> RegisterAsync(UserDto request);
    Task<TokenResponseDto?> LoginAsync(UserDto request);
    Task<User?> GetByIdAsync(int id);
    Task UpdateAsync(User user);
}
