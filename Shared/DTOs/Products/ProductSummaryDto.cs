namespace Ecommerce.Shared.DTOs.Products;

public class ProductSummaryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string? ImageUrl { get; set; }
    public string ShortDescription { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public IList<string> CategoryNames { get; set; } = [];
}
