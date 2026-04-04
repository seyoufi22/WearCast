using System.Security.Claims;
using WearCast.Api.Features.CartManagment.GetDesignsInCart.DTOs;

namespace WearCast.Api.Features.CartManagment.GetDesignsInCart;

[Tags("Cart")]
[Route("api/Cart")]
[ApiController]
public class GetCartEndpoint(ISender sender) : ControllerBase
{
    [Authorize(Roles = "Customer")]
    [HttpGet("GetDesignsInCart")]
    public async Task<IActionResult> GetCart(CancellationToken cancellationToken)
    {
        var customerId = User.FindFirstValue("CustomerId");
        if (string.IsNullOrEmpty(customerId))
            return Unauthorized();

        var query = new GetCartRequestDto(int.Parse(customerId));
        var result = await sender.Send(query, cancellationToken);
        return Ok(result);
    }
}