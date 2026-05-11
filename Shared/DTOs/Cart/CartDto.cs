namespace Ecommerce.Shared.DTOs.Cart;

public class CartDto
{
    public IList<CartItemDto> Items { get; set; } = [];
    public decimal Total => Items.Sum(i => i.Subtotal);
    public int ItemCount => Items.Sum(i => i.Quantity);
}
