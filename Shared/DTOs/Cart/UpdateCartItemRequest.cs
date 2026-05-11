using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Shared.DTOs.Cart;

public class UpdateCartItemRequest
{
    [Range(1, 100)]
    public int Quantity { get; set; }
}
