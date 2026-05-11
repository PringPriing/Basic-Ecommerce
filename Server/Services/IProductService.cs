using Ecommerce.Shared.DTOs.Common;
using Ecommerce.Shared.DTOs.Products;

namespace Ecommerce.Server.Services;

public interface IProductService
{
    Task<PagedResult<ProductSummaryDto>> SearchAsync(ProductSearchRequest request);
    Task<ProductDto?> GetByIdAsync(int id);
    Task<ProductDto> CreateAsync(CreateProductRequest request);
    Task<ProductDto?> UpdateAsync(int id, UpdateProductRequest request);
    Task<bool> DeleteAsync(int id);
    Task<ProductDto?> UpdateImageAsync(int id, string imageUrl, string blobName);
    Task<IList<ProductSummaryDto>> SuggestAsync(string query);
}
