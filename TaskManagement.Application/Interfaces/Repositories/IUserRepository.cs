using TaskManagement.Application.Contracts.Authentication;
using TaskManagement.Domain.Models;

namespace TaskManagement.Application.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User?> RegisterAsync(UserLoginRequest request);
    Task<TokenResponseDto?> LoginAsync(UserLoginRequest request);
    Task<User?> GetByIdAsync(int id);
    Task UpdateAsync(User user);
}
