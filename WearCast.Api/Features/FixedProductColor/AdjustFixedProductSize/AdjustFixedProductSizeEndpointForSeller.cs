using WearCast.Api.Features.FixedProductColor.AdjustFixedProductSize.DTOs;

namespace WearCast.Api.Features.FixedProductColor.UpdateFixedProductSizeQuantity;

[Tags("FixedProductColor")]
[Route("api/FixedProductColor")]
[ApiController]
public class AdjustFixedProductSizeEndpointForSeller : ControllerBase
{
    private readonly IMediator _mediator;

    public AdjustFixedProductSizeEndpointForSeller(IMediator mediator)
    {
        _mediator = mediator;
    }
    [Authorize(Roles = "Seller")] // مسموح فقط للبائع
    [HttpPost("SellerAdjustStock")]
    public async Task<IActionResult> SellerAdjust([FromBody] AdjustFixedProductSizeRequestDto request)
    {
        var isSuccess = await _mediator.Send(request);
        return isSuccess ? Ok("Inventory updated.") : BadRequest("Update failed.");
    }
}