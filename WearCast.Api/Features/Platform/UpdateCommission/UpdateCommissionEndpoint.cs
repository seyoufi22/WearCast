using WearCast.Api.Features.Platform.UpdateCommission.DTOs;

namespace WearCast.Api.Features.Platform.UpdateCommission;

[Route("api/platform")]
[ApiController]
[Authorize(Roles = DefaultRoles.SuperAdmin)]
[Tags("Platform")]
public class UpdateCommissionEndpoint(ISender sender) : ControllerBase
{
    [HttpPut("commission")]
    public async Task<IActionResult> UpdateCommission([FromBody] UpdateCommissionRequest request, CancellationToken cancellationToken)
    {
        var result = await sender.Send(request, cancellationToken);
        return result.ToResponse();
    }
}
