using System.Net;
using System.Net.Http.Json;
using Ecommerce.Server.Data;
using Ecommerce.Server.Data.Models;
using Ecommerce.Shared.DTOs.Auth;
using Ecommerce.Shared.DTOs.Common;
using Ecommerce.Shared.DTOs.Products;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace Ecommerce.Server.Tests.Integration;

public class ProductsControllerTests : IClassFixture<TestWebApplicationFactory>
{
    private readonly TestWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public ProductsControllerTests(TestWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    private async Task SeedProducts()
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        if (!db.Products.Any())
        {
            db.Products.AddRange(
                new Product { Name = "Laptop", Description = "A laptop", Price = 999m, IsActive = true },
                new Product { Name = "Phone", Description = "A phone", Price = 499m, IsActive = true },
                new Product { Name = "Tablet", Description = "A tablet", Price = 299m, IsActive = true }
            );
            await db.SaveChangesAsync();
        }
    }

    [Fact]
    public async Task GetProducts_ReturnsOk_WithPagedResult()
    {
        await SeedProducts();

        var response = await _client.GetAsync("api/products");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<PagedResult<ProductSummaryDto>>();
        body!.Items.Should().HaveCountGreaterThan(0);
    }

    [Fact]
    public async Task GetProducts_FiltersResultsByName()
    {
        await SeedProducts();

        var response = await _client.GetAsync("api/products?Name=Laptop");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<PagedResult<ProductSummaryDto>>();
        body!.Items.Should().AllSatisfy(p => p.Name.Should().Contain("Laptop"));
    }

    [Fact]
    public async Task GetProduct_ReturnsNotFound_WhenInvalidId()
    {
        var response = await _client.GetAsync("api/products/99999");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task CreateProduct_ReturnsUnauthorized_WhenNotLoggedIn()
    {
        var response = await _client.PostAsJsonAsync("api/products", new CreateProductRequest
        {
            Name = "Test", Description = "Test", Price = 10m
        });

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
