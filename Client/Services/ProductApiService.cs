using System.Net.Http.Json;
using Ecommerce.Shared.DTOs.Common;
using Ecommerce.Shared.DTOs.Products;

namespace Ecommerce.Client.Services;

public class ProductApiService
{
    private readonly HttpClient _http;

    public ProductApiService(HttpClient http) => _http = http;

    public async Task<PagedResult<ProductSummaryDto>?> SearchAsync(ProductSearchRequest request)
    {
        var query = $"api/products?Page={request.Page}&PageSize={request.PageSize}";
        if (!string.IsNullOrWhiteSpace(request.Name)) query += $"&Name={Uri.EscapeDataString(request.Name)}";
        if (request.CategoryId.HasValue) query += $"&CategoryId={request.CategoryId}";
        return await _http.GetFromJsonAsync<PagedResult<ProductSummaryDto>>(query);
    }

    public async Task<ProductDto?> GetByIdAsync(int id) =>
        await _http.GetFromJsonAsync<ProductDto>($"api/products/{id}");

    public async Task<IList<ProductSummaryDto>> SuggestAsync(string query)
    {
        if (string.IsNullOrWhiteSpace(query) || query.Length < 2)
            return [];
        return await _http.GetFromJsonAsync<IList<ProductSummaryDto>>(
            $"api/products/suggest?q={Uri.EscapeDataString(query)}") ?? [];
    }
}
