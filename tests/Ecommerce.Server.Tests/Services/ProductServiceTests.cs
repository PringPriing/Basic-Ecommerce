using Ecommerce.Server.Data;
using Ecommerce.Server.Data.Models;
using Ecommerce.Server.Services;
using Ecommerce.Shared.DTOs.Products;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Server.Tests.Services;

public class ProductServiceTests : IDisposable
{
    private readonly ApplicationDbContext _db;
    private readonly ProductService _sut;

    public ProductServiceTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _db = new ApplicationDbContext(options);
        _sut = new ProductService(_db);
    }

    [Fact]
    public async Task SearchAsync_ReturnsAllActiveProducts_WhenNoFilter()
    {
        _db.Products.AddRange(
            new Product { Name = "Widget", Description = "A widget", Price = 9.99m, IsActive = true },
            new Product { Name = "Gadget", Description = "A gadget", Price = 19.99m, IsActive = true },
            new Product { Name = "Hidden", Description = "Inactive", Price = 1m, IsActive = false }
        );
        await _db.SaveChangesAsync();

        var result = await _sut.SearchAsync(new ProductSearchRequest { Page = 1, PageSize = 10 });

        result.TotalCount.Should().Be(2);
        result.Items.Should().NotContain(p => p.Name == "Hidden");
    }

    [Fact]
    public async Task SearchAsync_FiltersProductsByName()
    {
        _db.Products.AddRange(
            new Product { Name = "Apple iPhone", Description = "Phone", Price = 999m, IsActive = true },
            new Product { Name = "Samsung Galaxy", Description = "Phone", Price = 899m, IsActive = true }
        );
        await _db.SaveChangesAsync();

        var result = await _sut.SearchAsync(new ProductSearchRequest { Name = "Apple", Page = 1, PageSize = 10 });

        result.TotalCount.Should().Be(1);
        result.Items[0].Name.Should().Be("Apple iPhone");
    }

    [Fact]
    public async Task SearchAsync_FiltersByCategory()
    {
        var electronics = new Category { Name = "Electronics", Description = "Tech" };
        var clothing = new Category { Name = "Clothing", Description = "Clothes" };
        _db.Categories.AddRange(electronics, clothing);

        var phone = new Product { Name = "Phone", Description = "A phone", Price = 500m, IsActive = true };
        var shirt = new Product { Name = "Shirt", Description = "A shirt", Price = 20m, IsActive = true };
        _db.Products.AddRange(phone, shirt);
        await _db.SaveChangesAsync();

        _db.ProductCategories.Add(new ProductCategory { ProductId = phone.Id, CategoryId = electronics.Id });
        _db.ProductCategories.Add(new ProductCategory { ProductId = shirt.Id, CategoryId = clothing.Id });
        await _db.SaveChangesAsync();

        var result = await _sut.SearchAsync(new ProductSearchRequest
        {
            CategoryId = electronics.Id,
            Page = 1,
            PageSize = 10
        });

        result.TotalCount.Should().Be(1);
        result.Items[0].Name.Should().Be("Phone");
    }

    [Fact]
    public async Task SearchAsync_PaginatesCorrectly()
    {
        for (int i = 1; i <= 15; i++)
            _db.Products.Add(new Product { Name = $"Product {i:D2}", Description = "Desc", Price = i, IsActive = true });
        await _db.SaveChangesAsync();

        var page1 = await _sut.SearchAsync(new ProductSearchRequest { Page = 1, PageSize = 10 });
        var page2 = await _sut.SearchAsync(new ProductSearchRequest { Page = 2, PageSize = 10 });

        page1.Items.Should().HaveCount(10);
        page1.TotalCount.Should().Be(15);
        page1.TotalPages.Should().Be(2);
        page2.Items.Should().HaveCount(5);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsProduct_WhenExists()
    {
        var product = new Product { Name = "Laptop", Description = "A laptop", Price = 1200m, IsActive = true };
        _db.Products.Add(product);
        await _db.SaveChangesAsync();

        var result = await _sut.GetByIdAsync(product.Id);

        result.Should().NotBeNull();
        result!.Name.Should().Be("Laptop");
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenNotFoundOrInactive()
    {
        _db.Products.Add(new Product { Name = "Inactive", Description = "Gone", Price = 1m, IsActive = false });
        await _db.SaveChangesAsync();

        var notFound = await _sut.GetByIdAsync(999);
        var inactive = await _sut.GetByIdAsync(1);

        notFound.Should().BeNull();
        inactive.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_SoftDeletesProduct()
    {
        var product = new Product { Name = "ToDelete", Description = "Bye", Price = 5m, IsActive = true };
        _db.Products.Add(product);
        await _db.SaveChangesAsync();

        var result = await _sut.DeleteAsync(product.Id);

        result.Should().BeTrue();
        (await _db.Products.FindAsync(product.Id))!.IsActive.Should().BeFalse();
    }

    public void Dispose() => _db.Dispose();
}
