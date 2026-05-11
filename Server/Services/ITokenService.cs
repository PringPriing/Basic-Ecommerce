using Ecommerce.Server.Data.Models;

namespace Ecommerce.Server.Services;

public interface ITokenService
{
    Task<string> GenerateTokenAsync(ApplicationUser user);
}
