using TaskManagement.Core.Dtos;
using TaskManagement.Core.Models;

namespace TaskManagement.Core.Interfaces;

public interface IUserRepository
{
    Task<User> Register(UserDto request);
}
