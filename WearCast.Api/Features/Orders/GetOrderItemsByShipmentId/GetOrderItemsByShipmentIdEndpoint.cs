using WearCast.Api.Common.Extensions;
using WearCast.Api.Features.Orders.GetOrderItemsByShipmentId.Query;

namespace WearCast.Api.Features.Orders.GetOrderItemsByShipmentId;

[Route("api/Orders")]
[ApiController]
[Tags("Order")]
public class GetOrderItemsByShipmentIdEndpoint : ControllerBase
{
    private readonly ISender _sender;

    public GetOrderItemsByShipmentIdEndpoint(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("shipment/{shipmentId}/items")]
    [Authorize]
    public async Task<IActionResult> Get(
        [FromRoute] int shipmentId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? searchTerm = null,
        [FromQuery] string? sortBy = null,
        [FromQuery] bool sortDescending = false)
    {
        int? customerId = User.IsInRole(DefaultRoles.Customer) ? User.GetCustomerId() : null;
        int? sellerId = User.IsInRole(DefaultRoles.SellerManager) ? User.GetSellerId() : null;
        int? factoryId = User.IsInRole(DefaultRoles.FactoryManager) ? User.GetFactoryId() : null;

        var request = new GetOrderItemsByShipmentIdQuery(
            shipmentId, customerId, sellerId, factoryId,
            pageNumber, pageSize, searchTerm, sortBy, sortDescending);

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
