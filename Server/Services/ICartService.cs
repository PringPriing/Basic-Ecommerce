using Ecommerce.Shared.DTOs.Cart;

namespace Ecommerce.Server.Services;

public interface ICartService
{
    Task<CartDto> GetCartAsync(string userId);
    Task<CartItemDto> AddItemAsync(string userId, AddToCartRequest request);
    Task<CartItemDto?> UpdateItemAsync(string userId, int itemId, UpdateCartItemRequest request);
    Task<bool> RemoveItemAsync(string userId, int itemId);
    Task ClearCartAsync(string userId);
}
