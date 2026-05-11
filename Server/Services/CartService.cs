using Ecommerce.Server.Data;
using Ecommerce.Server.Data.Models;
using Ecommerce.Shared.DTOs.Cart;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Server.Services;

public class CartService : ICartService
{
    private readonly ApplicationDbContext _db;

    public CartService(ApplicationDbContext db) => _db = db;

    public async Task<CartDto> GetCartAsync(string userId)
    {
        var items = await _db.CartItems
            .Where(ci => ci.UserId == userId)
            .Include(ci => ci.Product)
            .Select(ci => ToItemDto(ci))
            .ToListAsync();

        return new CartDto { Items = items };
    }

    public async Task<CartItemDto> AddItemAsync(string userId, AddToCartRequest request)
    {
        var existing = await _db.CartItems
            .Include(ci => ci.Product)
            .FirstOrDefaultAsync(ci => ci.UserId == userId && ci.ProductId == request.ProductId);

        if (existing != null)
        {
            existing.Quantity += request.Quantity;
            await _db.SaveChangesAsync();
            return ToItemDto(existing);
        }

        var cartItem = new CartItem
        {
            UserId = userId,
            ProductId = request.ProductId,
            Quantity = request.Quantity
        };
        _db.CartItems.Add(cartItem);
        await _db.SaveChangesAsync();

        await _db.Entry(cartItem).Reference(ci => ci.Product).LoadAsync();
        return ToItemDto(cartItem);
    }

    public async Task<CartItemDto?> UpdateItemAsync(string userId, int itemId, UpdateCartItemRequest request)
    {
        var item = await _db.CartItems
            .Include(ci => ci.Product)
            .FirstOrDefaultAsync(ci => ci.Id == itemId && ci.UserId == userId);

        if (item == null) return null;

        item.Quantity = request.Quantity;
        await _db.SaveChangesAsync();
        return ToItemDto(item);
    }

    public async Task<bool> RemoveItemAsync(string userId, int itemId)
    {
        var item = await _db.CartItems
            .FirstOrDefaultAsync(ci => ci.Id == itemId && ci.UserId == userId);

        if (item == null) return false;

        _db.CartItems.Remove(item);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task ClearCartAsync(string userId)
    {
        var items = await _db.CartItems.Where(ci => ci.UserId == userId).ToListAsync();
        _db.CartItems.RemoveRange(items);
        await _db.SaveChangesAsync();
    }

    private static CartItemDto ToItemDto(CartItem ci) => new()
    {
        Id = ci.Id,
        ProductId = ci.ProductId,
        ProductName = ci.Product?.Name ?? string.Empty,
        UnitPrice = ci.Product?.Price ?? 0,
        ProductImageUrl = ci.Product?.ImageUrl,
        Quantity = ci.Quantity
    };
}
