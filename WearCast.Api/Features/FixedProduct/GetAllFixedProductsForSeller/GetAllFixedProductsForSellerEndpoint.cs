using System.Security.Claims;
using WearCast.Api.Features.FixedProduct.GetAllFixedProductsForSeller.DTOs;

namespace WearCast.Api.Features.FixedProduct.GetAllFixedProductsForSeller;

[ApiController]
[Route("api/FixedProduct/GetAllFixedProductsForSeller")]
[Tags("FixedProduct")]
public class GetAllFixedProductsForSellerEndpoint : ControllerBase
{
    private readonly ISender _sender;

    public GetAllFixedProductsForSellerEndpoint(ISender sender)
    {
        _sender = sender;
    }
    [Authorize(Roles = "SellerManager")]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetAllFixedProductsForSellerRequestDto request, CancellationToken cancellationToken)
    {
        var sellerId = User.FindFirstValue("SellerId");

        if (string.IsNullOrEmpty(sellerId))
            return Unauthorized();

        request.SellerId = int.Parse(sellerId);

        var result = await _sender.Send(request, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }
}