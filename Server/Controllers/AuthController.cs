using Ecommerce.Server.Data.Models;
using Ecommerce.Server.Services;
using Ecommerce.Shared.DTOs.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Ecommerce.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private const string RefreshCookieName = "refreshToken";

    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly IConfiguration _config;

    public AuthController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        ITokenService tokenService,
        IConfiguration config)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _config = config;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register(RegisterRequest request)
    {
        if (await _userManager.FindByEmailAsync(request.Email) != null)
            return BadRequest(new { message = "Email is already registered." });

        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            EmailConfirmed = true
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
            return BadRequest(new { errors = result.Errors.Select(e => e.Description) });

        await _userManager.AddToRoleAsync(user, "Customer");

        var authResponse = await BuildAuthResponse(user);
        await AttachRefreshCookie(user.Id);
        return Ok(authResponse);
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login(LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
            return Unauthorized(new { message = "Invalid credentials." });

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: false);
        if (!result.Succeeded)
            return Unauthorized(new { message = "Invalid credentials." });

        var authResponse = await BuildAuthResponse(user);
        await AttachRefreshCookie(user.Id);
        return Ok(authResponse);
    }

    [AllowAnonymous]
    [HttpPost("refresh")]
    public async Task<ActionResult<AuthResponse>> Refresh()
    {
        var rawToken = Request.Cookies[RefreshCookieName];
        if (string.IsNullOrWhiteSpace(rawToken))
            return Unauthorized(new { message = "Refresh token missing." });

        try
        {
            var (user, newRaw) = await _tokenService.RotateRefreshTokenAsync(rawToken);
            SetRefreshCookie(newRaw);
            return Ok(await BuildAuthResponse(user));
        }
        catch (SecurityTokenException)
        {
            DeleteRefreshCookie();
            return Unauthorized(new { message = "Invalid or expired refresh token." });
        }
    }

    [AllowAnonymous]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var rawToken = Request.Cookies[RefreshCookieName];
        if (!string.IsNullOrWhiteSpace(rawToken))
            await _tokenService.RevokeRefreshTokenAsync(rawToken);

        DeleteRefreshCookie();
        return NoContent();
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<ActionResult<AuthResponse>> Me()
    {
        var user = await _userManager.FindByEmailAsync(User.Identity!.Name!);
        if (user == null) return NotFound();
        return Ok(await BuildAuthResponse(user));
    }

    private async Task<AuthResponse> BuildAuthResponse(ApplicationUser user)
    {
        var roles = await _userManager.GetRolesAsync(user);
        var token = await _tokenService.GenerateTokenAsync(user);
        var expiresMinutes = double.Parse(_config["Jwt:ExpiresMinutes"] ?? "15");
        return new AuthResponse
        {
            Token = token,
            Email = user.Email!,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Roles = roles,
            ExpiresAt = DateTime.UtcNow.AddMinutes(expiresMinutes)
        };
    }

    private async Task AttachRefreshCookie(string userId)
    {
        var rawToken = await _tokenService.CreateRefreshTokenAsync(userId);
        SetRefreshCookie(rawToken);
    }

    private void SetRefreshCookie(string rawToken)
    {
        var expiryDays = int.Parse(_config["Jwt:RefreshTokenExpiryDays"] ?? "7");
        Response.Cookies.Append(RefreshCookieName, rawToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddDays(expiryDays)
        });
    }

    private void DeleteRefreshCookie() =>
        Response.Cookies.Delete(RefreshCookieName, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict
        });
}
