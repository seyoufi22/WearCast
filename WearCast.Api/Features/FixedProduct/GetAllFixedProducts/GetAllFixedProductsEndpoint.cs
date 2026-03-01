using WearCast.Api.Features.FixedProduct.GetAllFixedProducts.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

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

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 100, CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new GetAllFixedProductsQuery(pageIndex, pageSize), cancellationToken);
        
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }
}
