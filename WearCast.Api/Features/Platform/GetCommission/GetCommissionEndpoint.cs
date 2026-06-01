using WearCast.Api.Features.Platform.GetCommission.DTOs;

namespace WearCast.Api.Features.Platform.GetCommission;

[Route("api/platform")]
[ApiController]
[Authorize(Roles = $"{DefaultRoles.SuperAdmin},{DefaultRoles.OperationsAdmin},{DefaultRoles.VendorAdmin},{DefaultRoles.CatalogAdmin},{DefaultRoles.CustomerServiceAdmin}")]
[Tags("Platform")]
public class GetCommissionEndpoint(ISender sender) : ControllerBase
{
    [HttpGet("commission")]
    public async Task<IActionResult> GetCommission(CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetCommissionRequest(), cancellationToken);
        return result.ToResponse();
    }
}
