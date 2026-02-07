using System.Runtime.CompilerServices;

using TaskManagement.Domain.Entities;

namespace TaskManagement.Application.Services;

public interface IJwtProvider
{
    (string token, int expiriesIn) GenerateTokenAsync(ApplicationUser user);
}
