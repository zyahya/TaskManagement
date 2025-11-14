using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using TaskManagement.Core.Dtos;
using TaskManagement.Core.Interfaces;
using TaskManagement.Core.Models;

namespace TaskManagement.Data.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;
    private readonly IAuthService _authService;
    private readonly IPasswordHasher<User> _passwordHasher;

    public UserRepository(AppDbContext context, IAuthService authService, IPasswordHasher<User> passwordHasher)
    {
        _context = context;
        _authService = authService;
        _passwordHasher = passwordHasher;
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return null;
        return user;
    }

    public async Task<TokenResponseDto?> LoginAsync(UserLoginDto request)
    {

        var user = await AuthenticateUser(request);
        if (user == null) return null;

        return await GenerateTokensAsync(user);
    }

    private async Task<User?> AuthenticateUser(UserLoginDto request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
        if (user == null) return null;

        var passwordVerification = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
        if (passwordVerification == PasswordVerificationResult.Failed)
        {
            return null;
        }

        return user;
    }

    private async Task<TokenResponseDto> GenerateTokensAsync(User user)
    {
        var accessToken = _authService.CreateToken(user);
        var refreshToken = _authService.GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

        await _context.SaveChangesAsync();

        return new TokenResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }

    public async Task<User?> RegisterAsync(UserLoginDto request)
    {
        if (await IsUserExistsAsync(request.Username))
        {
            return null;
        }

        var user = await CreateUserWithHashedPassword(request.Username, request.Password);

        return user;
    }

    private async Task<bool> IsUserExistsAsync(string username)
    {
        return await _context.Users.AnyAsync(u => u.Username == username);
    }

    private async Task<User> CreateUserWithHashedPassword(string username, string password)
    {
        var user = new User();
        var hashPassword = _passwordHasher.HashPassword(user, password);

        user.Username = username;
        user.PasswordHash = hashPassword;

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        return user;
    }

    public Task UpdateAsync(User user)
    {
        _context.Users.Update(user);
        return _context.SaveChangesAsync();
    }
}
