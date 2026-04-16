using System.Security.Claims;
using WearCast.Api.Features.Checkout.CreateCheckoutSession.DTOs;

namespace WearCast.Api.Features.Checkout.CreateCheckoutSession;

[ApiController]
[Route("api/Checkout")]
[Tags("Checkout")]
public class CreateCheckoutSessionEndpoint : ControllerBase
{
    private readonly ISender _sender;

    public CreateCheckoutSessionEndpoint(ISender sender)
    {
        _sender = sender;
    }

    [Authorize(Roles = DefaultRoles.Customer)]
    [HttpPost]
    public async Task<IActionResult> Checkout([FromBody] CreateCheckoutSessionRequestDto request, CancellationToken cancellationToken)
    {
        var customerId = User.GetCustomerId();
        if (customerId == null)
            return Unauthorized(new { Message = "CustomerId claim is missing from the token." });

        var email = User.FindFirstValue(ClaimTypes.Email) ?? string.Empty;

        request.CustomerId = customerId.Value;
        request.CustomerEmail = email;

        var result = await _sender.Send(request, cancellationToken);

        return result.IsSuccess
            ? Ok(result.Value)
            : BadRequest(result.Error);
    }
}

