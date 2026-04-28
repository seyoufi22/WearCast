using System.Security.Claims;
using WearCast.Api.Common.Enums;
using WearCast.Api.Common.Extensions;
using WearCast.Api.Features.Orders.UpdateOrderStatus.DTOs;

namespace WearCast.Api.Features.Orders.UpdateOrderStatus;

[ApiController]
[Route("api/Orders")]
[Tags("Order")]
public class UpdateOrderStatusEndpoint : ControllerBase
{
    private readonly ISender _sender;

    public UpdateOrderStatusEndpoint(ISender sender)
    {
        _sender = sender;
    }

    /// <summary>
    /// Updates order status.
    /// Seller: Paid → Ready | Driver: Ready → PickedUp
    /// </summary>
    [HttpPut("{orderId}/status")]
    [Authorize(Roles = $"{DefaultRoles.SellerManager},{DefaultRoles.Driver}")]
    public async Task<IActionResult> Update([FromRoute] int orderId, [FromBody] UpdateOrderStatusRequestDto requestDto, CancellationToken cancellationToken)
    {
        var role = User.FindFirstValue(ClaimTypes.Role);

        int? sellerId = null;
        int? driverId = null;

        if (role == DefaultRoles.SellerManager)
        {
            sellerId = User.GetSellerId();
            if (sellerId is null)
                return Unauthorized();
        }
        else if (role == DefaultRoles.Driver)
        {
            driverId = User.GetDriverId();
            if (driverId is null)
                return Unauthorized();
        }
        else
        {
            return Forbid();
        }

        var command = new UpdateOrderStatusCommand(orderId, requestDto.NewStatus, sellerId, driverId);
        var result = await _sender.Send(command, cancellationToken);

        if (!result.IsSuccess)
        {
            return result.Error.Code == "Orders.Forbidden"
                ? Forbid()
                : BadRequest(result.Error);
        }

        return Ok();
    }
}
