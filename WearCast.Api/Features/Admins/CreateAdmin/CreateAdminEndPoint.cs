namespace WearCast.Api.Features.Admins.CreateAdmin
{
    [Route("api/admins")]
    [ApiController]
    [Authorize(Roles = DefaultRoles.SuperAdmin)]
    [Tags("Admin Profile")]
    public class CreateAdminEndPoint(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAdminRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request, cancellationToken);

            return result.ToResponse();
        }
    }
}
