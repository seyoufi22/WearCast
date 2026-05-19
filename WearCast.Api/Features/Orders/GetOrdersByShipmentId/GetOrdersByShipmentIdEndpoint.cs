using WearCast.Api.Common.Extensions;
using WearCast.Api.Features.Orders.GetOrdersByShipmentId.Query;

namespace WearCast.Api.Features.Orders.GetOrdersByShipmentId;

[Route("api/Orders")]
[ApiController]
[Tags("Order")]
public class GetOrdersByShipmentIdEndpoint : ControllerBase
{
    private readonly ISender _sender;

    public GetOrdersByShipmentIdEndpoint(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("shipment/{shipmentId}")]
    [Authorize(Roles = $"{DefaultRoles.Customer},{DefaultRoles.SuperAdmin},{DefaultRoles.ShippingCompanyManager}, {DefaultRoles.Driver}")]
    public async Task<IActionResult> Get([FromRoute] int shipmentId)
    {
        var customerId = User.GetCustomerId();
        var isAdmin = User.IsInRole(DefaultRoles.SuperAdmin) || User.IsInRole(DefaultRoles.ShippingCompanyManager);

        var request = new GetOrdersByShipmentIdQuery(shipmentId, customerId, isAdmin);
        var result = await _sender.Send(request);

        if (result.IsFailure)
        {
            if (result.Error.Code == "Shipments.Forbidden")
                return Forbid();

            return NotFound(result.Error);
        }

        return Ok(result.Value);
    }
}
