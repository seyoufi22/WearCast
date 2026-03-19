using System.Security.Claims;
using WearCast.Api.Features.FixedProductColor.AddFixedProductImage.DTOs;
using WearCast.Api.Features.FixedProductColor.AddFixedProductSize.DTOs;
using WearCast.Api.Features.FixedProductSize.AddFixedProductSize.DTOs;


namespace WearCast.Api.Features.FixedProductSize.AddFixedProductSize;

[Tags("FixedProductColor")]
[Route("api/FixedProductColor")]
[ApiController]
public class AddFixedProductSizeEndpoint(ISender sender) : ControllerBase
{
    [Authorize(Roles = "SellerManager,SuperAdmin")]
    [HttpPost("AddSize")]
    public async Task<IActionResult> AddSize([FromBody] AddFixedProductSizeRequestDto request, CancellationToken cancellationToken)
    {
        Result result;
        var Role = User.FindFirstValue(ClaimTypes.Role);
        if (Role == "SuperAdmin")
            result = await sender.Send(new AddFixedProductSizeCommandDto(request, true, null));
        else
        {
            var sellerId = User.FindFirstValue("SellerId");

            if (string.IsNullOrEmpty(sellerId))
                return Unauthorized();

            result = await sender.Send(new AddFixedProductSizeCommandDto(request, false, int.Parse(sellerId)));
        }

        if (result.IsFailure)
        {
            return StatusCode(result.Error.StatusCode ?? StatusCodes.Status400BadRequest, result.Error);
        }
        return Ok(new { Message = "Size added successfully to the product color." });
    }
}