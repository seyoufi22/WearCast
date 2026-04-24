namespace WearCast.Api.Features.Admins.GetSuperAdmin;

[Route("api/admins")]
[ApiController]
[Authorize(Roles = DefaultRoles.SuperAdmin)]
[Tags("Admin Profile")]
public class GetSuperAdminEndPoint(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("super-admin/profile")]
    public async Task<IActionResult> GetProfile(CancellationToken cancellationToken)
    {
        var request = new GetSuperAdminRequest();

        var result = await _mediator.Send(request, cancellationToken);

        return result.ToResponse();
    }
}