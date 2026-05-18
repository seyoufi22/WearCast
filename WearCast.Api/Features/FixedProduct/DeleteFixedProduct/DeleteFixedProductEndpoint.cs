using System.Security.Claims;
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

    [Authorize(Roles = $"{DefaultRoles.SellerManager},{DefaultRoles.SuperAdmin},{DefaultRoles.CatalogAdmin}")]
    [HttpDelete]
    public async Task<IActionResult> Delete([FromBody] DeleteFixedProductRequest request, CancellationToken cancellationToken)
    {
        var Role = User.FindFirstValue(ClaimTypes.Role);
        if (Role == DefaultRoles.SuperAdmin || Role == DefaultRoles.CatalogAdmin)
            request.isAdminRequest = true;
        else
        {
            var sellerId = User.FindFirstValue("SellerId");

            if (string.IsNullOrEmpty(sellerId))
                return Unauthorized();
            request.SellerId = int.Parse(sellerId);
        }

        var result = await _sender.Send(request, cancellationToken);

        return result.IsSuccess ? NoContent() : BadRequest(result.Error);
    }
}
