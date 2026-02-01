using TaskManagement.Core.Contracts.Request;
using TaskManagement.Core.Dtos;
using TaskManagement.Core.Models;

namespace TaskManagement.Core.Interfaces;

public interface IUserRepository
{
    Task<User?> RegisterAsync(UserLoginRequest request);
    Task<TokenResponseDto?> LoginAsync(UserLoginRequest request);
    Task<User?> GetByIdAsync(int id);
    Task UpdateAsync(User user);
}
