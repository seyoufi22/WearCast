using System.Security.Claims;
using WearCast.Api.Features.FixedProductColor.AddFixedProductImage.DTOs;

namespace WearCast.Api.Features.FixedProductColor.AddFixedProductImage;

[Tags("FixedProductColor")]
[Route("api/FixedProductColor")]
[ApiController]
public class AddFixedProductImageEndpoint(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [Authorize(Roles = "SellerManager,SuperAdmin")]
    [HttpPost("AddImage")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> AddImage([FromForm] AddFixedProductImageRequestDto request)
    {
        Result result;
        var Role = User.FindFirstValue(ClaimTypes.Role);
        if (Role == "SuperAdmin")
            result = await _mediator.Send(new AddFixedProductImageCommandDto(request.ProductColorId, request.Image, 0, true));
        else
        {
            var sellerId = User.FindFirstValue("SellerId");

            if (string.IsNullOrEmpty(sellerId))
                return Unauthorized();

            result = await _mediator.Send(new AddFixedProductImageCommandDto(request.ProductColorId, request.Image, int.Parse(sellerId), false));
        }

        if (result.IsFailure)
        {
            return StatusCode(result.Error.StatusCode ?? StatusCodes.Status400BadRequest, result.Error);
        }
        return Ok(new { Message = "Image uploaded successfully." });
    }
}
