using WearCast.Api.Common.Extensions;
using WearCast.Api.Features.Orders.GetOrdersBySellerId.Query;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WearCast.Api.Features.Orders.GetOrdersBySellerId;

[Route("api/Orders")]
[ApiController]
public class GetOrdersBySellerIdEndpoint : ControllerBase
{
    private readonly ISender _sender;

    public GetOrdersBySellerIdEndpoint(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("seller")]
    [Authorize(Roles = "Seller")]
    public async Task<IActionResult> Get([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var sellerId = User.GetSellerId();

        var request = new GetOrdersBySellerIdQuery(sellerId.Value, pageNumber, pageSize);
        var result = await _sender.Send(request);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }
}
