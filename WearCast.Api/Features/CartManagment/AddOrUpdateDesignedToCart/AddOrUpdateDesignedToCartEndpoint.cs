using System.Security.Claims;
using WearCast.Api.Features.CartManagment.AddOrUpdateDesignedToCart.DTOs;
namespace WearCast.Api.Features.CartManagment.AddOrUpdateDesignedToCart;


[Tags("Cart")]
[Route("api/Cart")]
[ApiController]
public class AddOrUpdateDesignedToCartEndpoint(ISender sender) : ControllerBase
{
    [Authorize(Roles = "Customer")]
    [HttpPost("AddOrUpdateDesignedToCart")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update([FromBody] AddOrUpdateDesignedToCartRequest request, CancellationToken cancellationToken)
    {
        var customerId = User.FindFirstValue("CustomerId");

        if (string.IsNullOrEmpty(customerId))
            return Unauthorized();

        var command = new AddOrUpdateDesignedToCartCommand(request, int.Parse(customerId));

        var result = await sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return StatusCode(result.Error.StatusCode ?? StatusCodes.Status400BadRequest, result.Error);
        }
        return Ok(new { Message = "Cart updated successfully." });
    }
}
