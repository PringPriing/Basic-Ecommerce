using System.Security.Claims;
using Ecommerce.Server.Services;
using Ecommerce.Shared.DTOs.Cart;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CartController : ControllerBase
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService) => _cartService = cartService;

    private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    [HttpGet]
    public async Task<ActionResult<CartDto>> GetCart() =>
        Ok(await _cartService.GetCartAsync(UserId));

    [HttpPost]
    public async Task<ActionResult<CartItemDto>> AddItem(AddToCartRequest request)
    {
        return Ok(await _cartService.AddItemAsync(UserId, request));
    }

    [HttpPut("{itemId:int}")]
    public async Task<ActionResult<CartItemDto>> UpdateItem(int itemId, UpdateCartItemRequest request)
    {
        var updated = await _cartService.UpdateItemAsync(UserId, itemId, request);
        return updated == null ? NotFound() : Ok(updated);
    }

    [HttpDelete("{itemId:int}")]
    public async Task<IActionResult> RemoveItem(int itemId)
    {
        var removed = await _cartService.RemoveItemAsync(UserId, itemId);
        return removed ? NoContent() : NotFound();
    }

    [HttpDelete]
    public async Task<IActionResult> ClearCart()
    {
        await _cartService.ClearCartAsync(UserId);
        return NoContent();
    }
}
