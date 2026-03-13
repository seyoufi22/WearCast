using WearCast.Api.Features.FixedProductSize.AdjustFixedProductSizeQuantity.DTOs;

namespace WearCast.Api.Features.FixedProductSize.AdjustFixedProductSizeQuantity;

[Tags("FixedProductColor")]
[Route("api/FixedProductColor")]
[ApiController]
public class AdjustFixedProductSizeQuantityEndpoint(ISender sender) : ControllerBase
{
    [Authorize]
    [HttpPost("AdjustSizeQuantity")] 
    public async Task<IActionResult> AdjustSizeQuantity([FromBody] AdjustFixedProductSizeQuantityRequestDto request, CancellationToken cancellationToken)
    {
        var isSuccess = await sender.Send(request, cancellationToken);

        if (isSuccess)
        {
            return Ok(new { Message = "Size quantity adjusted successfully." });
        }

        return BadRequest(new { Message = "Failed to adjust. Product color not found." });
    }
}