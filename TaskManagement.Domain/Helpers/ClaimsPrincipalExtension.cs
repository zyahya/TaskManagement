using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace TaskManagement.Domain.Helpers;

public static class ClaimsPrincipalExtension
{
    public static bool TryGetUserId(this ClaimsPrincipal user, out int userId)
    {
        userId = default;

        if (user == null) return false;

        var idClaim = user.FindFirst(ClaimTypes.NameIdentifier) ?? user.FindFirst(JwtRegisteredClaimNames.Sub);
        if (idClaim == null) return false;

        return int.TryParse(idClaim.Value, out userId);
    }

    public static int GetUserIdOrThrow(this ClaimsPrincipal user)
    {
        if (user.TryGetUserId(out var id)) return id;
        throw new UnauthorizedAccessException("User id claim missing or invalid.");
    }
}
