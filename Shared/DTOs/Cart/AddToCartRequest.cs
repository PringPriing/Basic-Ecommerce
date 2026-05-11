using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Shared.DTOs.Cart;

public class AddToCartRequest
{
    [Required]
    public int ProductId { get; set; }

    [Range(1, 100)]
    public int Quantity { get; set; } = 1;
}
