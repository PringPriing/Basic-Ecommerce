using Ecommerce.Server.Data;
using Ecommerce.Server.Data.Models;
using Ecommerce.Shared.DTOs.Checkout;
using Microsoft.EntityFrameworkCore;
using Stripe;
using Stripe.Checkout;

namespace Ecommerce.Server.Services;

public class CheckoutService : ICheckoutService
{
    private readonly ApplicationDbContext _db;
    private readonly IConfiguration _config;

    public CheckoutService(ApplicationDbContext db, IConfiguration config)
    {
        _db = db;
        _config = config;
        StripeConfiguration.ApiKey = config["Stripe:SecretKey"];
    }

    public async Task<CreateCheckoutSessionResponse> CreateSessionAsync(string userId, string baseUrl)
    {
        var cartItems = await _db.CartItems
            .Where(ci => ci.UserId == userId)
            .Include(ci => ci.Product)
            .ToListAsync();

        if (cartItems.Count == 0)
            throw new InvalidOperationException("Cannot create a Stripe session for an empty cart.");

        var lineItems = cartItems.Select(ci => new SessionLineItemOptions
        {
            PriceData = new SessionLineItemPriceDataOptions
            {
                Currency = _config["Stripe:Currency"] ?? "usd",
                UnitAmount = (long)(ci.Product.Price * 100),
                ProductData = new SessionLineItemPriceDataProductDataOptions
                {
                    Name = ci.Product.Name,
                    Description = ci.Product.Description.Length > 0
                        ? ci.Product.Description[..Math.Min(ci.Product.Description.Length, 500)]
                        : null,
                }
            },
            Quantity = ci.Quantity,
        }).ToList();

        var options = new SessionCreateOptions
        {
            PaymentMethodTypes = ["card"],
            LineItems = lineItems,
            Mode = "payment",
            SuccessUrl = $"{baseUrl}/checkout/success?session_id={{CHECKOUT_SESSION_ID}}",
            CancelUrl = $"{baseUrl}/checkout/cancel",
            Metadata = new Dictionary<string, string> { ["userId"] = userId },
        };

        var service = new SessionService();
        var session = await service.CreateAsync(options);

        var order = new Order
        {
            UserId = userId,
            StripeSessionId = session.Id,
            Status = OrderStatus.PendingPayment,
            TotalAmount = cartItems.Sum(ci => ci.Product.Price * ci.Quantity),
            CreatedAt = DateTime.UtcNow,
            Items = cartItems.Select(ci => new OrderItem
            {
                ProductId = ci.ProductId,
                ProductName = ci.Product.Name,
                UnitPrice = ci.Product.Price,
                Quantity = ci.Quantity,
            }).ToList()
        };

        _db.Orders.Add(order);
        await _db.SaveChangesAsync();

        return new CreateCheckoutSessionResponse
        {
            SessionUrl = session.Url,
            SessionId = session.Id,
        };
    }

    public async Task HandleSessionCompletedAsync(Session session)
    {
        var order = await _db.Orders.FirstOrDefaultAsync(o => o.StripeSessionId == session.Id);

        if (order == null || order.Status == OrderStatus.Paid)
            return;

        order.Status = OrderStatus.Paid;
        order.StripePaymentIntentId = session.PaymentIntentId;
        order.PaidAt = DateTime.UtcNow;

        var userId = session.Metadata.TryGetValue("userId", out var uid) ? uid : order.UserId;
        var cartItems = await _db.CartItems.Where(ci => ci.UserId == userId).ToListAsync();
        _db.CartItems.RemoveRange(cartItems);

        await _db.SaveChangesAsync();
    }
}
