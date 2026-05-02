using WearCast.Api.Features.Platform.GetPlatformDashboard.DTOs;

namespace WearCast.Api.Features.Platform.GetPlatformDashboard;

[Route("api/platform")]
[ApiController]
[Authorize(Roles = $"{DefaultRoles.SuperAdmin},{DefaultRoles.OperationsAdmin}")]
[Tags("Platform")]
public class GetPlatformDashboardEndpoint(ISender sender) : ControllerBase
{
    [HttpGet("dashboard")]
    public async Task<IActionResult> GetDashboard(CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetPlatformDashboardRequest(), cancellationToken);
        return result.ToResponse();
    }
}
