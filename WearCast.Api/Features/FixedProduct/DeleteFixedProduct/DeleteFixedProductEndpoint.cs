using WearCast.Api.Features.FixedProduct.DeleteFixedProduct.DTOs;

namespace WearCast.Api.Features.FixedProduct.DeleteFixedProduct;

[ApiController]
[Route("api/FixedProduct/Delete")]
[Tags("FixedProduct")]
public class DeleteFixedProductEndpoint : ControllerBase
{
    private readonly ISender _sender;

    public DeleteFixedProductEndpoint(ISender sender)
    {
        _sender = sender;
    }

    [Authorize]
    [HttpDelete]
    public async Task<IActionResult> Delete([FromBody] DeleteFixedProductRequest request, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(request, cancellationToken);
        
        return result.IsSuccess ? NoContent() : BadRequest(result.Error);
    }
}
