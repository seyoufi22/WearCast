using WearCast.Api.Features.Checkout.GetShippingInfo.DTOs;

namespace WearCast.Api.Features.Checkout.GetShippingInfo;

[ApiController]
[Route("api/Checkout")]
[Tags("Checkout")]
public class GetShippingInfoEndpoint : ControllerBase
{
    private readonly ISender _sender;

    public GetShippingInfoEndpoint(ISender sender)
    {
        _sender = sender;
    }

    [Authorize(Roles = DefaultRoles.Customer)]
    [HttpGet("ShippingInfo")]
    public async Task<IActionResult> GetShippingInfo(CancellationToken cancellationToken)
    {
        var customerId = User.GetCustomerId();
        if (customerId == null)
            return Unauthorized(new { Message = "CustomerId claim is missing from the token." });

        var query = new GetShippingInfoRequestDto(customerId.Value);
        var result = await _sender.Send(query, cancellationToken);

        return result.IsSuccess
            ? Ok(result.Value)
            : result.Error.StatusCode == StatusCodes.Status404NotFound
                ? NotFound(result.Error)
                : BadRequest(result.Error);
    }
}
