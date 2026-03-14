using System.Security.Claims;
using WearCast.Api.Features.CartManagment.DeleteCartItem.DTOs;

namespace WearCast.Api.Features.CartManagment.DeleteCartItem;

[Tags("Cart")]
[Route("api/Cart")]
[ApiController]
public class DeleteCartItemEndpoint(ISender sender) : ControllerBase
{

    [HttpDelete("DeleteCartItem/{colorId}")]
    [Authorize] 
    public async Task<IActionResult> DeleteCartItem(int colorId, CancellationToken cancellationToken)
    {
        var customerId = User.FindFirstValue("CustomerId");

        if (string.IsNullOrEmpty(customerId))
            return Unauthorized();

        var command = new DeleteCartItemCommand(colorId, int.Parse(customerId));

        await sender.Send(command, cancellationToken);

        return Ok();
    }
}
