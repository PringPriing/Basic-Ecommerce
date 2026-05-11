using System.Net.Http.Json;
using Ecommerce.Shared.DTOs.Cart;

namespace Ecommerce.Client.Services;

public class CartApiService
{
    private readonly HttpClient _http;
    public event Action? CartChanged;

    public CartApiService(HttpClient http) => _http = http;

    public async Task<CartDto?> GetCartAsync() =>
        await _http.GetFromJsonAsync<CartDto>("api/cart");

    public async Task<CartItemDto?> AddItemAsync(AddToCartRequest request)
    {
        var response = await _http.PostAsJsonAsync("api/cart", request);
        if (!response.IsSuccessStatusCode) return null;
        var item = await response.Content.ReadFromJsonAsync<CartItemDto>();
        CartChanged?.Invoke();
        return item;
    }

    public async Task<bool> UpdateItemAsync(int itemId, UpdateCartItemRequest request)
    {
        var response = await _http.PutAsJsonAsync($"api/cart/{itemId}", request);
        if (response.IsSuccessStatusCode) CartChanged?.Invoke();
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> RemoveItemAsync(int itemId)
    {
        var response = await _http.DeleteAsync($"api/cart/{itemId}");
        if (response.IsSuccessStatusCode) CartChanged?.Invoke();
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> ClearCartAsync()
    {
        var response = await _http.DeleteAsync("api/cart");
        if (response.IsSuccessStatusCode) CartChanged?.Invoke();
        return response.IsSuccessStatusCode;
    }
}
