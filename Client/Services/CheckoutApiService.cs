using System.Net.Http.Json;
using Ecommerce.Shared.DTOs.Checkout;

namespace Ecommerce.Client.Services;

public class CheckoutApiService(HttpClient http)
{
    public async Task<CreateCheckoutSessionResponse?> CreateSessionAsync()
    {
        var response = await http.PostAsync("api/checkout/create-session", null);
        if (!response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync();
            throw new Exception($"Server returned {(int)response.StatusCode}: {body}");
        }
        return await response.Content.ReadFromJsonAsync<CreateCheckoutSessionResponse>();
    }
}
