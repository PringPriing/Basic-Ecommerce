using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using System.Security.Claims;
using Ecommerce.Client.Services;
using Ecommerce.Shared.DTOs.Auth;
using Microsoft.AspNetCore.Components.Authorization;

namespace Ecommerce.Client.Auth;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private const string TokenKey = "authToken";
    private static readonly TimeSpan RefreshBuffer = TimeSpan.FromMinutes(2);

    private readonly LocalStorageService _localStorage;
    private readonly IHttpClientFactory _httpClientFactory;

    public CustomAuthStateProvider(LocalStorageService localStorage, IHttpClientFactory httpClientFactory)
    {
        _localStorage = localStorage;
        _httpClientFactory = httpClientFactory;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await _localStorage.GetItemAsync(TokenKey);
        if (string.IsNullOrWhiteSpace(token))
            return Unauthenticated();

        JwtSecurityToken? jwt;
        try { jwt = new JwtSecurityTokenHandler().ReadJwtToken(token); }
        catch { return Unauthenticated(); }

        if (jwt.ValidTo < DateTime.UtcNow)
        {
            await _localStorage.RemoveItemAsync(TokenKey);
            return Unauthenticated();
        }

        var identity = new ClaimsIdentity(jwt.Claims, "jwt");
        return new AuthenticationState(new ClaimsPrincipal(identity));
    }

    public async Task<string?> GetTokenAsync()
    {
        var token = await _localStorage.GetItemAsync(TokenKey);
        if (string.IsNullOrWhiteSpace(token)) return null;

        JwtSecurityToken? jwt;
        try { jwt = new JwtSecurityTokenHandler().ReadJwtToken(token); }
        catch { return null; }

        if (jwt.ValidTo < DateTime.UtcNow)
        {
            await _localStorage.RemoveItemAsync(TokenKey);
            return null;
        }

        // Proactive refresh: silently rotate before the token actually expires
        if (jwt.ValidTo - DateTime.UtcNow < RefreshBuffer)
        {
            var refreshed = await TryRefreshInternalAsync();
            if (refreshed != null) return refreshed;
        }

        return token;
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

    // Returns the new access token on success, null on failure.
    private async Task<string?> TryRefreshInternalAsync()
    {
        try
        {
            var client = _httpClientFactory.CreateClient("RefreshClient");
            var response = await client.PostAsync("api/auth/refresh", null);
            if (!response.IsSuccessStatusCode) return null;

            var auth = await response.Content.ReadFromJsonAsync<AuthResponse>();
            if (auth?.Token == null) return null;

            await _localStorage.SetItemAsync(TokenKey, auth.Token);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
            return auth.Token;
        }
        catch
        {
            return null;
        }
    }

    private static AuthenticationState Unauthenticated() =>
        new(new ClaimsPrincipal(new ClaimsIdentity()));
}
