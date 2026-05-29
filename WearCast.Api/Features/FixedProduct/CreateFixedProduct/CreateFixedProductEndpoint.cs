using MediatR;
using Microsoft.AspNetCore.Mvc;
using WearCast.Api.Features.FixedProduct.CreateProduct.DTOs;

namespace WearCast.Api.Features.FixedProduct.CreateProduct;

[ApiController]
[Route("api/FixedProduct/Create")]
[Tags("FixedProduct")]
public class CreateFixedProductEndpoint : ControllerBase
{
    private readonly ISender _sender;

    public CreateFixedProductEndpoint(ISender sender)
    {
        _sender = sender;
    }
    [Authorize(Roles = DefaultRoles.SellerManager)]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateFixedProductRequestDto request, CancellationToken cancellationToken)
    {
        request.CreatedById = User.GetUserId()!;
        
        var sellerId = User.GetSellerId();
        if (sellerId == null)
            return Unauthorized(new { Message = "SellerId claim is missing from the token." });

        request.SellerId = sellerId.Value;

        var result = await _sender.Send(request, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }
}
