namespace WearCast.Api.Features.Drivers.UpdateDriverImage
{
    [Route("api/driver/profile-image")]
    [ApiController]
    [Authorize(Roles = $"{DefaultRoles.Driver},{DefaultRoles.SuperAdmin}")]
    [Tags("Driver Profile")]
    [Consumes("multipart/form-data")]
    public class UpdateDriverImageEndPoint(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPut]
        public async Task<IActionResult> Update([FromForm] UpdateDriverImageRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request, cancellationToken);

            return result.ToResponse();
        }
    }
}
