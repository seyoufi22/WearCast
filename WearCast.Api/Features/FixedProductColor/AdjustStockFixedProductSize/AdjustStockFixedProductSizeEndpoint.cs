using System.Security.Claims;
using WearCast.Api.Features.FixedProductColor.AddFixedProductImage.DTOs;
using WearCast.Api.Features.FixedProductColor.AdjustStockFixedProductSize.DTOs;
using WearCast.Api.Features.FixedProductSize.AdjustStockFixedProductSize.DTOs;

namespace WearCast.Api.Features.FixedProductSize.AdjustStockFixedProductSize;

[Tags("FixedProductColor")]
[Route("api/FixedProductColor")]
[ApiController]
public class AdjustStockFixedProductSizeEndpoint(ISender sender) : ControllerBase
{
    [Authorize(Roles = "SellerManager,SuperAdmin")]
    [HttpPost("AdjustSizeQuantity")] 
    public async Task<IActionResult> AdjustSizeQuantity([FromBody] AdjustStockFixedProductSizeRequestDto request, CancellationToken cancellationToken)
    {
        Result result;
        var Role = User.FindFirstValue(ClaimTypes.Role);
        if (Role == "SuperAdmin")
            result = await sender.Send(new AdjustStockFixedProductSizeCommandDto(request, 0, true));
        else
        {
            var sellerId = User.FindFirstValue("SellerId");

            if (string.IsNullOrEmpty(sellerId))
                return Unauthorized();

            result = await sender.Send(new AdjustStockFixedProductSizeCommandDto(request, int.Parse(sellerId), false));
        }

        if (result.IsFailure)
        {
            return StatusCode(result.Error.StatusCode ?? StatusCodes.Status400BadRequest, result.Error);
        }
        return Ok(new { Message = "Size quantity adjusted successfully." });
    }
}