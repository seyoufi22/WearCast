using System.Security.Claims;
using WearCast.Api.Features.CartManagment.DeleteCartItem.DTOs;

namespace WearCast.Api.Features.CartManagment.DeleteCartItem;

[Tags("Cart")]
[Route("api/Cart")]
[ApiController]
public class DeleteCartItemEndpoint(ISender sender) : ControllerBase
{

    [Authorize(Roles = "Customer")] 
    [HttpDelete("DeleteCartItem/{CartItemId}")]
    public async Task<IActionResult> DeleteCartItem([FromRoute] int CartItemId, CancellationToken cancellationToken)
    {
        var customerId = User.FindFirstValue("CustomerId");

        if (string.IsNullOrEmpty(customerId))
            return Unauthorized();

        var command = new DeleteCartItemCommand(CartItemId, int.Parse(customerId));

        var result = await sender.Send(command, cancellationToken);
        if (result.IsFailure)
        {
            return StatusCode(result.Error.StatusCode ?? StatusCodes.Status400BadRequest, result.Error);
        }
        return Ok();
    }
}
