using Ecommerce.Server.Data;
using Ecommerce.Server.Data.Models;
using Ecommerce.Server.Services;
using Ecommerce.Shared.DTOs.Categories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Server.Tests.Services;

public class CategoryServiceTests : IDisposable
{
    private readonly ApplicationDbContext _db;
    private readonly CategoryService _sut;

    public CategoryServiceTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _db = new ApplicationDbContext(options);
        _sut = new CategoryService(_db);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsOnlyActiveCategories()
    {
        _db.Categories.AddRange(
            new Category { Name = "Active", Description = "Yes", IsActive = true },
            new Category { Name = "Inactive", Description = "No", IsActive = false }
        );
        await _db.SaveChangesAsync();

        var result = await _sut.GetAllAsync();

        result.Should().HaveCount(1);
        result[0].Name.Should().Be("Active");
    }

    [Fact]
    public async Task CreateAsync_PersistsCategory()
    {
        var request = new CreateCategoryRequest { Name = "Books", Description = "All books" };

        var result = await _sut.CreateAsync(request);

        result.Id.Should().BeGreaterThan(0);
        result.Name.Should().Be("Books");
        _db.Categories.Should().HaveCount(1);
    }

    [Fact]
    public async Task UpdateAsync_ModifiesExistingCategory()
    {
        var category = new Category { Name = "Old", Description = "Old desc" };
        _db.Categories.Add(category);
        await _db.SaveChangesAsync();

        var request = new UpdateCategoryRequest { Name = "New", Description = "New desc", IsActive = true };
        var result = await _sut.UpdateAsync(category.Id, request);

        result.Should().NotBeNull();
        result!.Name.Should().Be("New");
    }

    [Fact]
    public async Task UpdateAsync_ReturnsNull_WhenNotFound()
    {
        var result = await _sut.UpdateAsync(999, new UpdateCategoryRequest { Name = "X", Description = "Y" });

        result.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_SoftDeletesCategory()
    {
        var category = new Category { Name = "ToDelete", Description = "Bye" };
        _db.Categories.Add(category);
        await _db.SaveChangesAsync();

        var result = await _sut.DeleteAsync(category.Id);

        result.Should().BeTrue();
        (await _db.Categories.FindAsync(category.Id))!.IsActive.Should().BeFalse();
    }

    [Fact]
    public async Task DeleteAsync_ReturnsFalse_WhenNotFound()
    {
        var result = await _sut.DeleteAsync(999);

        result.Should().BeFalse();
    }

    public void Dispose() => _db.Dispose();
}
