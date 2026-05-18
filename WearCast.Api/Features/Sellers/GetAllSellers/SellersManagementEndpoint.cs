using WearCast.Api.Common.Views;
using WearCast.Api.Features.Sellers.GetAllSellers;

namespace WearCast.Api.Features.Sellers.GetSeller;

[Route("api/sellers")]
[ApiController]
[Authorize(Roles = $"{DefaultRoles.SuperAdmin},{DefaultRoles.VendorAdmin}")]
[Tags("Seller Profile")]
public class SellersManagementEndpoint(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("allForAdmin")]
    public async Task<IActionResult> GetAllSellers([FromQuery] GetAllSellersRequest query, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(query, cancellationToken);

        return result.ToResponse();
    }
}