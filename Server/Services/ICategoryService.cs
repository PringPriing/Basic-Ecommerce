using Ecommerce.Shared.DTOs.Categories;

namespace Ecommerce.Server.Services;

public interface ICategoryService
{
    Task<IList<CategoryDto>> GetAllAsync();
    Task<CategoryDto?> GetByIdAsync(int id);
    Task<CategoryDto> CreateAsync(CreateCategoryRequest request);
    Task<CategoryDto?> UpdateAsync(int id, UpdateCategoryRequest request);
    Task<bool> DeleteAsync(int id);
}
