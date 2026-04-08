namespace WearCast.Api.Features.Admins.UpdateSuperAdmin
{
    [Route("api/admins")]
    [ApiController]
    [Authorize(Roles = DefaultRoles.SuperAdmin)]
    [Tags("Admin Profile")]
    public class UpdateSuperAdminEndPoint(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPut("super-admin/profile")]
        public async Task<IActionResult> Update([FromBody] UpdateSuperAdminRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request, cancellationToken);

            return result.ToResponse();
        }
    }
}
