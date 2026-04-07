using WearCast.Api.Common.Enums;
using WearCast.Api.Common.Extensions;
using WearCast.Api.Features.Orders.GetOrdersBySellerId.Query;

namespace WearCast.Api.Features.Orders.GetOrdersBySellerId;

[Route("api/Orders")]
[ApiController]
[Tags("Order")]
public class GetOrdersBySellerIdEndpoint : ControllerBase
{
    private readonly ISender _sender;

    public GetOrdersBySellerIdEndpoint(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("seller")]
    [Authorize(Roles = "Seller")]
    public async Task<IActionResult> Get(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] OrderStatus? statusFilter = null,
        [FromQuery] string? sortBy = null,
        [FromQuery] bool sortDescending = true)
    {
        var sellerId = User.GetSellerId();

        var request = new GetOrdersBySellerIdQuery(sellerId.Value, pageNumber, pageSize, statusFilter, sortBy, sortDescending);
        var result = await _sender.Send(request);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }
}
