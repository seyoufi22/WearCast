using System.Security.Claims;
using WearCast.Api.Features.FixedProductColor.AdjustStockFixedProductSize.DTOs;
using WearCast.Api.Features.FixedProductColor.DeleteFixedProductColor.DTOs;

namespace WearCast.Api.Features.FixedProductColor.DeleteFixedProductColor;

[Tags("FixedProductColor")]
[Route("api/FixedProductColor")]
[ApiController]
public class DeleteFixedProductColorEndpoint(ISender sender) : ControllerBase
{
    [Authorize(Roles = "SellerManager,SuperAdmin")]
    [HttpDelete("DeleteColor/{ColorId:int}", Name = "DeleteFixedProductColor")]
    public async Task<IActionResult> DeleteColor(int ColorId, CancellationToken cancellationToken)
    {
        Result result;
        var Role = User.FindFirstValue(ClaimTypes.Role);
        if (Role == "SuperAdmin")
            result = await sender.Send(new DeleteFixedProductColorRequestDto(ColorId, 0, true));
        else
        {
            var sellerId = User.FindFirstValue("SellerId");

            if (string.IsNullOrEmpty(sellerId))
                return Unauthorized();

            result = await sender.Send(new DeleteFixedProductColorRequestDto(ColorId, int.Parse(sellerId), false));
        }

        if (result.IsFailure)
        {
            return StatusCode(result.Error.StatusCode ?? StatusCodes.Status400BadRequest, result.Error);
        }
        return Ok(new { Message = "Product color deleted successfully." });
    }
}