using WearCast.Api.Features.FixedProduct.GetAllFixedProductsForAdmin.DTOs;

namespace WearCast.Api.Features.FixedProduct.GetAllFixedProductsForAdmin;

[ApiController]
[Route("api/FixedProduct")] 
[Tags("FixedProduct")]
public class GetAllFixedProductsForAdminEndpoint : ControllerBase
{
    private readonly ISender _sender;

    public GetAllFixedProductsForAdminEndpoint(ISender sender)
    {
        _sender = sender;
    }

    [Authorize(Roles = $"{DefaultRoles.SuperAdmin},{DefaultRoles.CatalogAdmin}")]
    [HttpGet("GetAllForAdmin")]
    public async Task<IActionResult> GetAll([FromQuery] GetAllFixedProductsForAdminRequestDto request, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(request, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }
}