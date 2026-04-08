using WearCast.Api.Features.FixedProduct.GetFixedProductById.DTOs;

namespace WearCast.Api.Features.FixedProduct.GetFixedProductById;

[ApiController]
[Route("api/FixedProduct/GetById")]
[Tags("FixedProduct")]
public class GetFixedProductByIdEndpoint : ControllerBase
{
    private readonly ISender _sender;

    public GetFixedProductByIdEndpoint(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetFixedProductByIdQuery(id), cancellationToken);
        
        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
    }
}
