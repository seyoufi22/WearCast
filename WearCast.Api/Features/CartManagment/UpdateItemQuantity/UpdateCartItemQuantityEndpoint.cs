using System.Security.Claims;
using WearCast.Api.Features.CartManagment.UpdateCartItemQuantity.DTOs;

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
        // إرسال الريكويست مباشرة للـ MediatR (سيتم جلب الـ ID داخل الهاندلر)
        var result = await sender.Send(request, cancellationToken);

        if (result.IsFailure)
        {
            return StatusCode(result.Error.StatusCode ?? StatusCodes.Status400BadRequest, result.Error);
        }

        return Ok(new { Message = "Quantity updated successfully." });
    }
}