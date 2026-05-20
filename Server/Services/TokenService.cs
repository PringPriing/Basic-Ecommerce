using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Ecommerce.Server.Data;
using Ecommerce.Server.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Ecommerce.Server.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _config;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _context;

    public TokenService(IConfiguration config, UserManager<ApplicationUser> userManager, ApplicationDbContext context)
    {
        _config = config;
        _userManager = userManager;
        _context = context;
    }

    public async Task<string> GenerateTokenAsync(ApplicationUser user)
    {
        var roles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Email, user.Email!),
            new(ClaimTypes.GivenName, user.FirstName),
            new(ClaimTypes.Surname, user.LastName)
        };

        claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.UtcNow.AddMinutes(double.Parse(_config["Jwt:ExpiresMinutes"] ?? "15"));

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: expires,
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<string> CreateRefreshTokenAsync(string userId)
    {
        var rawToken = Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
        var hash = HashToken(rawToken);

        var stale = _context.RefreshTokens
            .Where(rt => rt.UserId == userId && (rt.IsRevoked || rt.ExpiresAt < DateTime.UtcNow));
        _context.RefreshTokens.RemoveRange(stale);

        var expiryDays = int.Parse(_config["Jwt:RefreshTokenExpiryDays"] ?? "7");
        _context.RefreshTokens.Add(new RefreshToken
        {
            TokenHash = hash,
            UserId = userId,
            ExpiresAt = DateTime.UtcNow.AddDays(expiryDays)
        });

        await _context.SaveChangesAsync();
        return rawToken;
    }

    public async Task<(ApplicationUser User, string NewRawToken)> RotateRefreshTokenAsync(string rawToken)
    {
        var hash = HashToken(rawToken);

        var token = await _context.RefreshTokens
            .Include(rt => rt.User)
            .FirstOrDefaultAsync(rt => rt.TokenHash == hash);

        if (token == null || token.IsRevoked || token.ExpiresAt < DateTime.UtcNow)
            throw new SecurityTokenException("Invalid or expired refresh token.");

        token.IsRevoked = true;
        await _context.SaveChangesAsync();

        var newRaw = await CreateRefreshTokenAsync(token.UserId);
        return (token.User, newRaw);
    }

    public async Task RevokeRefreshTokenAsync(string rawToken)
    {
        var hash = HashToken(rawToken);

        var token = await _context.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.TokenHash == hash);

        if (token != null)
        {
            token.IsRevoked = true;
            await _context.SaveChangesAsync();
        }
    }

    private static string HashToken(string raw) =>
        Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(raw)));
}
