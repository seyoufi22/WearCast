using WearCast.Api.Features.FixedProduct.UpdateFixedProduct.DTOs;

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

    [Authorize]
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateFixedProductRequestDto request, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(request, cancellationToken);
        
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }
}
