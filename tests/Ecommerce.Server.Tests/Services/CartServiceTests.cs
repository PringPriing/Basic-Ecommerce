using Ecommerce.Server.Data;
using Ecommerce.Server.Data.Models;
using Ecommerce.Server.Services;
using Ecommerce.Shared.DTOs.Cart;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Server.Tests.Services;

public class CartServiceTests : IDisposable
{
    private readonly ApplicationDbContext _db;
    private readonly CartService _sut;
    private const string UserId = "user-001";

    public CartServiceTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _db = new ApplicationDbContext(options);
        _sut = new CartService(_db);

        var user = new ApplicationUser { Id = UserId, UserName = "test@test.com", Email = "test@test.com" };
        _db.Users.Add(user);
        _db.SaveChanges();
    }

    private async Task<Product> SeedProduct(string name, decimal price)
    {
        var product = new Product { Name = name, Description = "Desc", Price = price, IsActive = true };
        _db.Products.Add(product);
        await _db.SaveChangesAsync();
        return product;
    }

    [Fact]
    public async Task GetCartAsync_ReturnsEmptyCart_WhenNoItems()
    {
        var cart = await _sut.GetCartAsync(UserId);

        cart.Items.Should().BeEmpty();
        cart.Total.Should().Be(0);
    }

    [Fact]
    public async Task AddItemAsync_AddsNewItemToCart()
    {
        var product = await SeedProduct("Widget", 9.99m);

        await _sut.AddItemAsync(UserId, new AddToCartRequest { ProductId = product.Id, Quantity = 2 });
        var cart = await _sut.GetCartAsync(UserId);

        cart.Items.Should().HaveCount(1);
        cart.Items[0].Quantity.Should().Be(2);
        cart.Items[0].UnitPrice.Should().Be(9.99m);
    }

    [Fact]
    public async Task AddItemAsync_IncrementsQuantity_WhenProductAlreadyInCart()
    {
        var product = await SeedProduct("Widget", 5m);
        await _sut.AddItemAsync(UserId, new AddToCartRequest { ProductId = product.Id, Quantity = 1 });
        await _sut.AddItemAsync(UserId, new AddToCartRequest { ProductId = product.Id, Quantity = 3 });

        var cart = await _sut.GetCartAsync(UserId);

        cart.Items.Should().HaveCount(1);
        cart.Items[0].Quantity.Should().Be(4);
    }

    [Fact]
    public async Task UpdateItemAsync_ChangesQuantity()
    {
        var product = await SeedProduct("Gadget", 10m);
        var item = await _sut.AddItemAsync(UserId, new AddToCartRequest { ProductId = product.Id, Quantity = 1 });

        var updated = await _sut.UpdateItemAsync(UserId, item.Id, new UpdateCartItemRequest { Quantity = 5 });

        updated.Should().NotBeNull();
        updated!.Quantity.Should().Be(5);
    }

    [Fact]
    public async Task UpdateItemAsync_ReturnsNull_ForWrongUser()
    {
        var product = await SeedProduct("Gadget", 10m);
        var item = await _sut.AddItemAsync(UserId, new AddToCartRequest { ProductId = product.Id, Quantity = 1 });

        var result = await _sut.UpdateItemAsync("wrong-user", item.Id, new UpdateCartItemRequest { Quantity = 5 });

        result.Should().BeNull();
    }

    [Fact]
    public async Task RemoveItemAsync_RemovesItemFromCart()
    {
        var product = await SeedProduct("Thing", 7m);
        var item = await _sut.AddItemAsync(UserId, new AddToCartRequest { ProductId = product.Id, Quantity = 1 });

        var removed = await _sut.RemoveItemAsync(UserId, item.Id);
        var cart = await _sut.GetCartAsync(UserId);

        removed.Should().BeTrue();
        cart.Items.Should().BeEmpty();
    }

    [Fact]
    public async Task ClearCartAsync_RemovesAllItems()
    {
        var p1 = await SeedProduct("P1", 1m);
        var p2 = await SeedProduct("P2", 2m);
        await _sut.AddItemAsync(UserId, new AddToCartRequest { ProductId = p1.Id, Quantity = 1 });
        await _sut.AddItemAsync(UserId, new AddToCartRequest { ProductId = p2.Id, Quantity = 1 });

        await _sut.ClearCartAsync(UserId);
        var cart = await _sut.GetCartAsync(UserId);

        cart.Items.Should().BeEmpty();
    }

    [Fact]
    public async Task GetCartAsync_CalculatesTotalCorrectly()
    {
        var p1 = await SeedProduct("Cheap", 5m);
        var p2 = await SeedProduct("Expensive", 20m);
        await _sut.AddItemAsync(UserId, new AddToCartRequest { ProductId = p1.Id, Quantity = 3 });
        await _sut.AddItemAsync(UserId, new AddToCartRequest { ProductId = p2.Id, Quantity = 2 });

        var cart = await _sut.GetCartAsync(UserId);

        cart.Total.Should().Be(55m); // 3*5 + 2*20
    }

    public void Dispose() => _db.Dispose();
}
