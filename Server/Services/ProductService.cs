using Ecommerce.Server.Data;
using Ecommerce.Server.Data.Models;
using Ecommerce.Shared.DTOs.Categories;
using Ecommerce.Shared.DTOs.Common;
using Ecommerce.Shared.DTOs.Products;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Server.Services;

public class ProductService : IProductService
{
    private readonly ApplicationDbContext _db;

    public ProductService(ApplicationDbContext db) => _db = db;

    public async Task<PagedResult<ProductSummaryDto>> SearchAsync(ProductSearchRequest request)
    {
        var query = _db.Products
            .Where(p => p.IsActive)
            .Include(p => p.ProductCategories)
                .ThenInclude(pc => pc.Category)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Name))
            query = query.Where(p => p.Name.Contains(request.Name));

        if (request.CategoryId.HasValue)
            query = query.Where(p => p.ProductCategories.Any(pc => pc.CategoryId == request.CategoryId));

        var total = await query.CountAsync();
        var items = await query
            .OrderBy(p => p.Name)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(p => new ProductSummaryDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                ImageUrl = p.ImageUrl,
                ShortDescription = p.Description.Length > 150
                    ? p.Description.Substring(0, 150) + "..."
                    : p.Description,
                IsActive = p.IsActive,
                CategoryNames = p.ProductCategories.Select(pc => pc.Category.Name).ToList()
            })
            .ToListAsync();

        return new PagedResult<ProductSummaryDto>
        {
            Items = items,
            TotalCount = total,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }

    public async Task<ProductDto?> GetByIdAsync(int id)
    {
        var product = await _db.Products
            .Include(p => p.ProductCategories)
                .ThenInclude(pc => pc.Category)
            .FirstOrDefaultAsync(p => p.Id == id && p.IsActive);

        return product == null ? null : ToDto(product);
    }

    public async Task<ProductDto> CreateAsync(CreateProductRequest request)
    {
        var product = new Product
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            StockQuantity = request.StockQuantity
        };

        _db.Products.Add(product);
        await _db.SaveChangesAsync();

        await AttachCategoriesAsync(product.Id, request.CategoryIds);
        await _db.SaveChangesAsync();

        return (await GetByIdAsync(product.Id))!;
    }

    public async Task<ProductDto?> UpdateAsync(int id, UpdateProductRequest request)
    {
        var product = await _db.Products
            .Include(p => p.ProductCategories)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null) return null;

        product.Name = request.Name;
        product.Description = request.Description;
        product.Price = request.Price;
        product.StockQuantity = request.StockQuantity;
        product.IsActive = request.IsActive;

        _db.ProductCategories.RemoveRange(product.ProductCategories);
        await AttachCategoriesAsync(product.Id, request.CategoryIds);

        await _db.SaveChangesAsync();
        return (await GetByIdAsync(product.Id))!;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var product = await _db.Products.FindAsync(id);
        if (product == null) return false;

        product.IsActive = false;
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<ProductDto?> UpdateImageAsync(int id, string imageUrl, string blobName)
    {
        var product = await _db.Products.FindAsync(id);
        if (product == null) return null;

        product.ImageUrl = imageUrl;
        product.ImageBlobName = blobName;
        await _db.SaveChangesAsync();
        return await GetByIdAsync(id);
    }

    public async Task<IList<ProductSummaryDto>> SuggestAsync(string query)
    {
        return await _db.Products
            .Where(p => p.IsActive && p.Name.Contains(query))
            .OrderBy(p => p.Name)
            .Take(5)
            .Select(p => new ProductSummaryDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                ImageUrl = p.ImageUrl
            })
            .ToListAsync();
    }

    private async Task AttachCategoriesAsync(int productId, IList<int> categoryIds)
    {
        foreach (var catId in categoryIds.Distinct())
        {
            if (await _db.Categories.AnyAsync(c => c.Id == catId && c.IsActive))
            {
                _db.ProductCategories.Add(new ProductCategory
                {
                    ProductId = productId,
                    CategoryId = catId
                });
            }
        }
    }

    private static ProductDto ToDto(Product p) => new()
    {
        Id = p.Id,
        Name = p.Name,
        Description = p.Description,
        Price = p.Price,
        StockQuantity = p.StockQuantity,
        ImageUrl = p.ImageUrl,
        IsActive = p.IsActive,
        CreatedAt = p.CreatedAt,
        Categories = p.ProductCategories.Select(pc => new CategoryDto
        {
            Id = pc.Category.Id,
            Name = pc.Category.Name,
            Description = pc.Category.Description,
            IsActive = pc.Category.IsActive
        }).ToList()
    };
}
