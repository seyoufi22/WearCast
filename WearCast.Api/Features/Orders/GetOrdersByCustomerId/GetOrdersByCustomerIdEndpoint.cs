using WearCast.Api.Common.Enums;
using WearCast.Api.Common.Extensions;
using WearCast.Api.Features.Orders.GetOrdersByCustomerId.Query;

namespace WearCast.Api.Features.Orders.GetOrdersByCustomerId;

[Route("api/Orders")]
[ApiController]
[Tags("Order")]
public class GetOrdersByCustomerIdEndpoint : ControllerBase
{
    private readonly ISender _sender;

    public GetOrdersByCustomerIdEndpoint(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("customer")]
    [Authorize(Roles = DefaultRoles.Customer)]
    public async Task<IActionResult> Get(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] OrderStatus? statusFilter = null,
        [FromQuery] string? sortBy = null,
        [FromQuery] bool sortDescending = true)
    {
        var customerId = User.GetCustomerId();

        var request = new GetOrdersByCustomerIdQuery(customerId.Value, pageNumber, pageSize, statusFilter, sortBy, sortDescending);
        var result = await _sender.Send(request);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }
}
