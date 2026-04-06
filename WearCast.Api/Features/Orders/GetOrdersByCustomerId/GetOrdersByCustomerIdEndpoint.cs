using WearCast.Api.Common.Extensions;
using WearCast.Api.Features.Orders.GetOrdersByCustomerId.Query;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WearCast.Api.Features.Orders.GetOrdersByCustomerId;

[Route("api/Orders")]
[ApiController]
public class GetOrdersByCustomerIdEndpoint : ControllerBase
{
    private readonly ISender _sender;

    public GetOrdersByCustomerIdEndpoint(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("customer")]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> Get([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var customerId = User.GetCustomerId();

        var request = new GetOrdersByCustomerIdQuery(customerId.Value, pageNumber, pageSize);
        var result = await _sender.Send(request);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }
}
