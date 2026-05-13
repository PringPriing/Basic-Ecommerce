namespace Ecommerce.Server.Data.Models;

public enum OrderStatus { PendingPayment, Paid, Cancelled }

public class Order
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public ApplicationUser User { get; set; } = null!;
    public string StripeSessionId { get; set; } = string.Empty;
    public string? StripePaymentIntentId { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.PendingPayment;
    public decimal TotalAmount { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? PaidAt { get; set; }
    public ICollection<OrderItem> Items { get; set; } = [];
}
