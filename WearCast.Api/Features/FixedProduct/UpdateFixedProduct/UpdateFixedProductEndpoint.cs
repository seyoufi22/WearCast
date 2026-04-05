using System.Security.Claims;
using WearCast.Api.Features.FixedProduct.UpdateFixedProduct.DTOs;
using WearCast.Api.Features.FixedProductColor.UpdateFixedProductColor.DTOs;

namespace WearCast.Api.Features.FixedProduct.UpdateFixedProduct;

[ApiController]
[Route("api/FixedProduct/Update")]
[Tags("FixedProduct")]
public class UpdateFixedProductEndpoint : ControllerBase
{
    private readonly ISender _sender;

    public UpdateFixedProductEndpoint(ISender sender)
    {
        _sender = sender;
    }

    [Authorize(Roles = "SellerManager,SuperAdmin")]
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateFixedProductRequestDto request, CancellationToken cancellationToken)
    {
        var Role = User.FindFirstValue(ClaimTypes.Role);
        if (Role == "SuperAdmin")
            request.isAdminRequest = true;
        else
        {
            var sellerId = User.FindFirstValue("SellerId");

            if (string.IsNullOrEmpty(sellerId))
                return Unauthorized();
            request.SellerId = int.Parse(sellerId);
        }

        var result = await _sender.Send(request, cancellationToken);
        
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }
}
