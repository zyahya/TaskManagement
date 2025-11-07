using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

using TaskManagement.Core.Dtos;
using TaskManagement.Core.Interfaces;
using TaskManagement.Core.Models;
using TaskManagement.Core.Services;


namespace TaskManagement.Data.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;
    private readonly IJwtService _jwtService;

    public UserRepository(AppDbContext context, IJwtService jwtService)
    {
        _context = context;
        _jwtService = jwtService;
    }

    public async Task<string?> LoginAsync(UserDto request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
        if (user == null)
        {
            return null;
        }

        var passwordVerification = new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, request.Password);
        if (passwordVerification == PasswordVerificationResult.Failed)
        {
            return null;
        }

        var token = _jwtService.CreateToken(user);

        return token;
    }

    public async Task<User?> RegisterAsync(UserDto request)
    {
        if (await _context.Users.AnyAsync(u => u.Username == request.Username))
        {
            return null;
        }

        var user = new User();
        var hashPassword = new PasswordHasher<User>().HashPassword(user, request.Password);

        user.Username = request.Username;
        user.PasswordHash = hashPassword;

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        return user;
    }
}
