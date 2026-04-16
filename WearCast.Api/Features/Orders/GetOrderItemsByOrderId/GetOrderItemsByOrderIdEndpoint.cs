using WearCast.Api.Common.Extensions;
using WearCast.Api.Features.Orders.GetOrderItemsByOrderId.Query;

namespace WearCast.Api.Features.Orders.GetOrderItemsByOrderId;

[Route("api/Orders")]
[ApiController]
[Tags("Order")]
public class GetOrderItemsByOrderIdEndpoint : ControllerBase
{
    private readonly ISender _sender;

    public GetOrderItemsByOrderIdEndpoint(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("{orderId}/items")]
    [Authorize]
    public async Task<IActionResult> Get([FromRoute] int orderId)
    {
        int? customerId = User.IsInRole(DefaultRoles.Customer) ? User.GetCustomerId() : null;
        int? sellerId = User.IsInRole(DefaultRoles.SellerManager) ? User.GetSellerId() : null;
        int? factoryId = User.IsInRole(DefaultRoles.FactoryManager) ? User.GetFactoryId() : null;

        var request = new GetOrderItemsByOrderIdQuery(orderId, customerId, sellerId, factoryId);
        var result = await _sender.Send(request);

        if (result.IsFailure)
        {
            if (result.Error.Code == "Orders.Forbidden")
                return Forbid();
                
            return NotFound(result.Error);
        }

        return Ok(result.Value);
    }
}
