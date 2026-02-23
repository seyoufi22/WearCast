using WearCast.Api.Features.FixedProduct.CreateProduct.DTOs;
using WearCast.Api.Features.FixedProduct.CreateProduct.DTOs;

namespace WearCast.Api.Features.FixedProduct.CreateProduct;

[ApiController]
[Route("api/createFixedProduct")]
[Tags("FixedProduct")]
public class CreateFixedProductEndpoint : ControllerBase
{
    private readonly ISender _sender;

    public CreateFixedProductEndpoint(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateFixedProductRequestDto request, CancellationToken cancellationToken)
    {
        request.CreatedById = User.GetUserId()!;
        var result = await _sender.Send(request, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }
}
