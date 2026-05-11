namespace Ecommerce.Shared.DTOs.Products;

public class ProductSearchRequest
{
    public string? Name { get; set; }
    public int? CategoryId { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 12;
}
