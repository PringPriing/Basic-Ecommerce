using Ecommerce.Shared.DTOs.Checkout;
using Stripe.Checkout;

namespace Ecommerce.Server.Services;

public interface ICheckoutService
{
    Task<CreateCheckoutSessionResponse> CreateSessionAsync(string userId, string baseUrl);
    Task HandleSessionCompletedAsync(Session session);
}
