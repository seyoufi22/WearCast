namespace WearCast.Api.Features.Admins.CreateSuperAdmin
{
    [Route("api/admins")]
    [ApiController]
    [Authorize(Roles = DefaultRoles.SuperAdmin)]
    [Tags("Admin Profile")]
    public class CreateSuperAdminEndPoint(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost("super-admin")]
        public async Task<IActionResult> CreateSuperAdmin([FromBody] CreateSuperAdminRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request, cancellationToken);

            return result.ToResponse();
        }
    }
}
