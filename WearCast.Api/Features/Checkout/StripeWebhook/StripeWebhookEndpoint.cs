using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;
using WearCast.Api.Common.Settings;
using WearCast.Api.Features.Checkout.StripeWebhook.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WearCast.Api.Features.Checkout.StripeWebhook;

[Route("api/Webhook/Stripe")]
[ApiController]
public class StripeWebhookEndpoint : ControllerBase
{
    private readonly IOptions<StripeSettings> _stripeSettings;
    private readonly ISender _sender;

    public StripeWebhookEndpoint(IOptions<StripeSettings> stripeSettings, ISender sender)
    {
        _stripeSettings = stripeSettings;
        _sender = sender;
    }

    [HttpPost]
    public async Task<IActionResult> HandleWebhook(CancellationToken cancellationToken)
    {
        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync(cancellationToken);
        
        try
        {
            var stripeEvent = EventUtility.ConstructEvent(
                json, 
                Request.Headers["Stripe-Signature"], 
                _stripeSettings.Value.WebhookSecret,
                throwOnApiVersionMismatch: false
            );

            if (stripeEvent.Type == EventTypes.CheckoutSessionCompleted)
            {
                var session = stripeEvent.Data.Object as Session;
                if (session != null)
                {
                    await _sender.Send(new StripeWebhookRequestDto(session.Id), cancellationToken);
                }
            }

            return Ok();
        }
        catch (StripeException)
        {
            return BadRequest();
        }
    }
}
