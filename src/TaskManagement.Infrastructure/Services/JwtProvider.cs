using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using TaskManagement.Application.Services;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Infrastructure.Services;

public class JwtProvider : IJwtProvider
{
    private readonly JwtOptions _jwtOptions;

    public JwtProvider(IOptions<JwtOptions> jwtOptions)
    {
        _jwtOptions = jwtOptions.Value;
    }

    public (string token, int expiriesIn) GenerateTokenAsync(ApplicationUser user)
    {
        var secretKey = _jwtOptions.Key;
        var issuer = _jwtOptions.Issuer;
        var audience = _jwtOptions.Audience;
        var expiresMinutes = _jwtOptions.ExpiriesMinutes;

        Claim[] claims = [
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.GivenName, user.FirstName),
            new(JwtRegisteredClaimNames.FamilyName, user.LastName),
            new(JwtRegisteredClaimNames.Email, user.Email!),
        ];

        var keyBytes = Encoding.ASCII.GetBytes(secretKey);
        var signingKey = new SymmetricSecurityKey(keyBytes);

        var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            signingCredentials: credentials,
            expires: DateTime.UtcNow.AddMinutes(expiresMinutes)
        );

        return (
            token: new JwtSecurityTokenHandler().WriteToken(token),
            expiriesIn: expiresMinutes * 60
        );
    }
}
