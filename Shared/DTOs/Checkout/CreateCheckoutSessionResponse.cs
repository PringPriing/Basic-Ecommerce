namespace Ecommerce.Shared.DTOs.Checkout;

public class CreateCheckoutSessionResponse
{
    public string SessionUrl { get; set; } = string.Empty;
    public string SessionId { get; set; } = string.Empty;
}
