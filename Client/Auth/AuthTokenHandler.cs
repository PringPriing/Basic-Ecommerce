using System.Net;
using System.Net.Http.Headers;

namespace Ecommerce.Client.Auth;

public class AuthTokenHandler : DelegatingHandler
{
    private readonly CustomAuthStateProvider _authProvider;

    public AuthTokenHandler(CustomAuthStateProvider authProvider) =>
        _authProvider = authProvider;

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = await _authProvider.GetTokenAsync();
        if (!string.IsNullOrWhiteSpace(token))
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await base.SendAsync(request, cancellationToken);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
            await _authProvider.MarkAsLoggedOutAsync();

        return response;
    }
}
