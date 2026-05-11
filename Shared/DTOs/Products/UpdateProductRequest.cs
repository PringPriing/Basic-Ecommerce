using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Shared.DTOs.Products;

public class UpdateProductRequest
{
    [Required, MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string Description { get; set; } = string.Empty;

    [Required, Range(0.01, double.MaxValue)]
    public decimal Price { get; set; }

    [Range(0, int.MaxValue)]
    public int StockQuantity { get; set; }

    public bool IsActive { get; set; } = true;

    public IList<int> CategoryIds { get; set; } = [];
}
