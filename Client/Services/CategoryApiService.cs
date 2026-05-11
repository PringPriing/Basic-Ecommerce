using System.Net.Http.Json;
using Ecommerce.Shared.DTOs.Categories;

namespace Ecommerce.Client.Services;

public class CategoryApiService
{
    private readonly HttpClient _http;

    public CategoryApiService(HttpClient http) => _http = http;

    public async Task<IList<CategoryDto>?> GetAllAsync() =>
        await _http.GetFromJsonAsync<IList<CategoryDto>>("api/categories");

    public async Task<CategoryDto?> CreateAsync(CreateCategoryRequest request)
    {
        var response = await _http.PostAsJsonAsync("api/categories", request);
        return response.IsSuccessStatusCode
            ? await response.Content.ReadFromJsonAsync<CategoryDto>()
            : null;
    }

    public async Task<bool> UpdateAsync(int id, UpdateCategoryRequest request)
    {
        var response = await _http.PutAsJsonAsync($"api/categories/{id}", request);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var response = await _http.DeleteAsync($"api/categories/{id}");
        return response.IsSuccessStatusCode;
    }
}
