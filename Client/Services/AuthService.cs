using System.Net.Http.Json;
using System.Text.Json;
using Ecommerce.Shared.DTOs.Auth;

namespace Ecommerce.Client.Services;

public class AuthService
{
    private readonly HttpClient _http;

    public AuthService(HttpClient http) => _http = http;

    public async Task<(AuthResponse? Result, IEnumerable<string>? Errors)> RegisterAsync(RegisterRequest request)
    {
        var response = await _http.PostAsJsonAsync("api/auth/register", request);
        if (response.IsSuccessStatusCode)
            return (await response.Content.ReadFromJsonAsync<AuthResponse>(), null);

        var json = await response.Content.ReadFromJsonAsync<JsonElement>();
        if (json.TryGetProperty("errors", out var errorsEl))
            return (null, errorsEl.EnumerateArray().Select(e => e.GetString()!));
        if (json.TryGetProperty("message", out var messageEl))
            return (null, [messageEl.GetString()!]);

        return (null, ["Registration failed."]);
    }

    public async Task<AuthResponse?> LoginAsync(LoginRequest request)
    {
        var response = await _http.PostAsJsonAsync("api/auth/login", request);
        if (response.IsSuccessStatusCode)
            return await response.Content.ReadFromJsonAsync<AuthResponse>();
        return null;
    }
}
