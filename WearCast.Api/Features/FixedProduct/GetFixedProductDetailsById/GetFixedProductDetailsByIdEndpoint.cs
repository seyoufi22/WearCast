using WearCast.Api.Features.FixedProduct.GetFixedProductDetailsById.DTOs;

namespace WearCast.Api.Features.FixedProduct.GetFixedProductDetailsById;

[ApiController]
[Route("api/FixedProduct/GetDetailsById")]
[Tags("FixedProduct")]
public class GetFixedProductDetailsByIdEndpoint : ControllerBase
{
    private readonly ISender _sender;

    public GetFixedProductDetailsByIdEndpoint(ISender sender)
    {
        _sender = sender;
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetDetailsById([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetFixedProductDetailsByIdQuery(id), cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
    }
}
