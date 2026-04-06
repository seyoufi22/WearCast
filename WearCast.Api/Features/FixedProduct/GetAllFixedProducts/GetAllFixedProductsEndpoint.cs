using WearCast.Api.Features.FixedProduct.GetAllFixedProducts.DTOs;


namespace WearCast.Api.Features.FixedProduct.GetAllFixedProducts;

[ApiController]
[Route("api/FixedProduct/GetAll")]
[Tags("FixedProduct")]
public class GetAllFixedProductsEndpoint : ControllerBase
{
    private readonly ISender _sender;

    public GetAllFixedProductsEndpoint(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetAllFixedProductsQuery request,CancellationToken cancellationToken)
    {

        var result = await _sender.Send(request, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }
}
