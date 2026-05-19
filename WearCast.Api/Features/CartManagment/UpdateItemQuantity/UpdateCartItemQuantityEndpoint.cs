using System.Security.Claims;
using WearCast.Api.Features.CartManagment.UpdateCartItemQuantity.DTOs;
using WearCast.Api.Features.CartManagment.GetMyCart.DTOs; // <-- Essential for GetCartRequestDto

namespace WearCast.Api.Features.CartManagment.UpdateCartItemQuantity;

[Tags("Cart")]
[Route("api/Cart")]
[ApiController]
public class UpdateCartItemQuantityEndpoint(ISender sender) : ControllerBase
{
    [Authorize(Roles = "Customer")]
    [HttpPut("UpdateItemQuantity")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update([FromBody] UpdateCartItemQuantityRequest request, CancellationToken cancellationToken)
    {
        var customerId = User.FindFirstValue("CustomerId");
        if (string.IsNullOrEmpty(customerId))
            return Unauthorized();

        // 1. Send the update command to the Handler (increment or decrement)
        var updateResult = await sender.Send(request, cancellationToken);

        if (updateResult.IsFailure)
        {
            return StatusCode(updateResult.Error.StatusCode ?? StatusCodes.Status400BadRequest, updateResult.Error);
        }

        // 2. After a successful update, fetch the fully updated cart data
        var getCartQuery = new GetCartRequestDto(int.Parse(customerId));
        var updatedCart = await sender.Send(getCartQuery, cancellationToken);

        // 3. Return the updated cart data to the frontend (instead of a text message)
        return Ok(updatedCart);
    }
}