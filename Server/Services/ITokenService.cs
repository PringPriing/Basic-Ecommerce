using Ecommerce.Server.Data.Models;

namespace Ecommerce.Server.Services;

public interface ITokenService
{
    Task<string> GenerateTokenAsync(ApplicationUser user);
    Task<string> CreateRefreshTokenAsync(string userId);
    Task<(ApplicationUser User, string NewRawToken)> RotateRefreshTokenAsync(string rawToken);
    Task RevokeRefreshTokenAsync(string rawToken);
}
