using System.Security.Claims;
using WearCast.Api.Features.FixedProduct.GetAllFixedProductsForSeller.DTOs;
using WearCast.Api.Features.FixedProduct.GetAllFixedProductsStatusForSeller.DTOs;

namespace WearCast.Api.Features.FixedProduct.GetAllFixedProductsStatusForSeller;

//public class GetAllFixedProductsStatusForSellerEndpoint
//{
//}
[ApiController]
[Route("api/FixedProduct/GetAllFixedProductsStatusForSeller")]
[Tags("FixedProduct")]
public class GetAllFixedProductsStatusForSellerEndpoint : ControllerBase
{
    private readonly ISender _sender;

    public GetAllFixedProductsStatusForSellerEndpoint(ISender sender)
    {
        _sender = sender;
    }
    [Authorize(Roles = "SellerManager")]
    [HttpGet]
    public async Task<IActionResult> GetAll( CancellationToken cancellationToken)
    {
        var sellerId = User.FindFirstValue("SellerId");

        if (string.IsNullOrEmpty(sellerId))
            return Unauthorized();

        var result = await _sender.Send(new GetAllFixedProductsStatusForSellerRequestDto(int.Parse(sellerId)), cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }
}