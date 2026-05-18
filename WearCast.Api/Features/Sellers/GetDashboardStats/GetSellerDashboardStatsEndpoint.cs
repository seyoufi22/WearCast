namespace WearCast.Api.Features.Sellers.GetDashboardStats;

[Route("api/sellers")]
[ApiController]
[Authorize(Roles = $"{DefaultRoles.SellerManager},{DefaultRoles.SuperAdmin},{DefaultRoles.VendorAdmin}")]
[Tags("Seller Profile")]
public class GetSellerDashboardStatsEndpoint(ISender sender) : ControllerBase
{
    [HttpGet("dashboard-stats")]
    public async Task<IActionResult> GetDashboardStats(CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetSellerDashboardStatsRequest(), cancellationToken);
        return result.ToResponse();
    }
}