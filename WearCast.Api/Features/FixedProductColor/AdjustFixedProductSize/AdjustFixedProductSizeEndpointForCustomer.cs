using WearCast.Api.Features.FixedProductColor.AdjustFixedProductSize.DTOs;

namespace WearCast.Api.Features.FixedProductColor.AdjustFixedProductSize;

[Tags("FixedProductColor")]
[Route("api/FixedProductColor")]
[ApiController]
public class AdjustFixedProductSizeEndpointForCustomer : ControllerBase
{
    private readonly IMediator _mediator;

    public AdjustFixedProductSizeEndpointForCustomer(IMediator mediator)
    {
        _mediator = mediator;
    }
    [Authorize(Roles = "Customer")] 
    [HttpPost("CustomerDeductStock")]
    public async Task<IActionResult> CustomerDeduct([FromBody] AdjustFixedProductSizeRequestDto request)
    {
        if (request.Amount > 0) request.Amount *= -1;

        var isSuccess = await _mediator.Send(request);
        return isSuccess ? Ok("Item reserved.") : BadRequest("Item out of stock.");
    }
}