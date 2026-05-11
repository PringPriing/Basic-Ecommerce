using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Ecommerce.Client.Services;
using Microsoft.AspNetCore.Components.Authorization;

namespace Ecommerce.Client.Auth;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private const string TokenKey = "authToken";
    private readonly LocalStorageService _localStorage;

    public CustomAuthStateProvider(LocalStorageService localStorage) =>
        _localStorage = localStorage;

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await _localStorage.GetItemAsync(TokenKey);
        if (string.IsNullOrWhiteSpace(token))
            return Unauthenticated();

        var handler = new JwtSecurityTokenHandler();
        JwtSecurityToken? jwt;
        try
        {
            jwt = handler.ReadJwtToken(token);
        }
        catch
        {
            return Unauthenticated();
        }

        if (jwt.ValidTo < DateTime.UtcNow)
        {
            await _localStorage.RemoveItemAsync(TokenKey);
            return Unauthenticated();
        }

        var identity = new ClaimsIdentity(jwt.Claims, "jwt");
        var user = new ClaimsPrincipal(identity);
        return new AuthenticationState(user);
    }

    public async Task MarkAsAuthenticatedAsync(string token)
    {
        await _localStorage.SetItemAsync(TokenKey, token);
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public async Task MarkAsLoggedOutAsync()
    {
        await _localStorage.RemoveItemAsync(TokenKey);
        NotifyAuthenticationStateChanged(Task.FromResult(Unauthenticated()));
    }

    public async Task<string?> GetTokenAsync() =>
        await _localStorage.GetItemAsync(TokenKey);

    private static AuthenticationState Unauthenticated() =>
        new(new ClaimsPrincipal(new ClaimsIdentity()));
}
