using System.Security.Claims;
using Ecommerce.Server.Services;
using Ecommerce.Shared.DTOs.Checkout;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace Ecommerce.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CheckoutController : ControllerBase
{
    private readonly ICheckoutService _checkoutService;
    private readonly IConfiguration _config;
    private readonly ILogger<CheckoutController> _logger;

    public CheckoutController(ICheckoutService checkoutService, IConfiguration config, ILogger<CheckoutController> logger)
    {
        _checkoutService = checkoutService;
        _config = config;
        _logger = logger;
    }

    private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    [HttpPost("create-session")]
    [Authorize]
    public async Task<ActionResult<CreateCheckoutSessionResponse>> CreateSession()
    {
        try
        {
            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            var response = await _checkoutService.CreateSessionAsync(UserId, baseUrl);
            return Ok(response);
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("empty cart"))
        {
            return BadRequest(new { message = "Your cart is empty." });
        }
        catch (StripeException ex)
        {
            _logger.LogError(ex, "Stripe API error during session creation for user {UserId}", UserId);
            return StatusCode(502, new { message = "Payment provider error. Please try again." });
        }
    }

    [HttpPost("webhook")]
    public async Task<IActionResult> Webhook()
    {
        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
        var webhookSecret = _config["Stripe:WebhookSecret"]!;

        Stripe.Event stripeEvent;
        try
        {
            stripeEvent = EventUtility.ConstructEvent(
                json,
                Request.Headers["Stripe-Signature"],
                webhookSecret,
                throwOnApiVersionMismatch: false);
        }
        catch (StripeException ex)
        {
            _logger.LogWarning(ex, "Stripe webhook signature verification failed");
            return BadRequest(new { message = "Invalid webhook signature." });
        }

        if (stripeEvent.Type == "checkout.session.completed")
        {
            var session = stripeEvent.Data.Object as Stripe.Checkout.Session;
            if (session != null)
            {
                try
                {
                    await _checkoutService.HandleSessionCompletedAsync(session);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error handling checkout.session.completed for session {SessionId}", session.Id);
                    return StatusCode(500);
                }
            }
        }

        return Ok();
    }
}
