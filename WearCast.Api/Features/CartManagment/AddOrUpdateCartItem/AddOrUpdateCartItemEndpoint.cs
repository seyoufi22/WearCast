using System.Security.Claims;
using WearCast.Api.Features.CartManagement.AddOrUpdateCartItem.DTOs;
using WearCast.Api.Features.CartManagment.AddOrUpdateCartItem.DTOs;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
namespace WearCast.Api.Features.CartManagment.AddOrUpdateCartItem;

[Tags("Cart")]
[Route("api/Cart")]
[ApiController]
public class AddOrUpdateCartItemEndPoint(ISender sender) : ControllerBase
{
    [Authorize]
    [HttpPost("AddOrUpdateItem")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update([FromBody] AddOrUpdateCartItemRequest request, CancellationToken cancellationToken)
    {
        var customerId = User.FindFirstValue("CustomerId");

        if (string.IsNullOrEmpty(customerId))
            return Unauthorized();

        var command = new AddOrUpdateCartItemCommand(request, int.Parse(customerId));

        var result = await sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return StatusCode(result.Error.StatusCode ?? StatusCodes.Status400BadRequest, result.Error);
        }
        return Ok(new { Message = "Cart updated successfully." });
    }
}
