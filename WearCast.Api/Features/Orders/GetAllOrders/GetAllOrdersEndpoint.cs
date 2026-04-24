using WearCast.Api.Common.Enums;
using WearCast.Api.Common.Extensions;
using WearCast.Api.Features.Orders.GetAllOrders.Query;

namespace WearCast.Api.Features.Orders.GetAllOrders;

[Route("api/Orders")]
[ApiController]
[Tags("Order")]
public class GetAllOrdersEndpoint : ControllerBase
{
    private readonly ISender _sender;

    public GetAllOrdersEndpoint(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("GetAllByID")]
    [Authorize]
    public async Task<IActionResult> Get(
        [FromQuery] int? sellerId = null,
        [FromQuery] int? factoryId = null,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] OrderStatus? statusFilter = null,
        [FromQuery] string? sortBy = null,
        [FromQuery] bool sortDescending = true)
    {
        int? resolvedSellerId = null;
        int? resolvedFactoryId = null;

        if (User.IsInRole(DefaultRoles.SellerManager))
        {
            resolvedSellerId = User.GetSellerId();
        }
        else if (User.IsInRole(DefaultRoles.FactoryManager))
        {
            resolvedFactoryId = User.GetFactoryId();
        }
        else if (User.IsInRole(DefaultRoles.SuperAdmin))
        {
            // Admin can query by any seller or factory
            resolvedSellerId = sellerId;
            resolvedFactoryId = factoryId;
        }
        else
        {
            return Forbid();
        }

        var request = new GetAllOrdersQuery(
            resolvedSellerId, resolvedFactoryId,
            pageNumber, pageSize, statusFilter, sortBy, sortDescending);

        var result = await _sender.Send(request);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }
}
