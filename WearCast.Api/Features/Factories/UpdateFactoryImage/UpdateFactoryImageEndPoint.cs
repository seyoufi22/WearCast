namespace WearCast.Api.Features.Factories.UpdateFactoryImage
{
    [Route("api/factories/profile-image")]
    [ApiController]
    [Authorize(Roles = $"{DefaultRoles.FactoryManager},{DefaultRoles.VendorAdmin},{DefaultRoles.SuperAdmin}")]
    [Tags("Factory Profile")]
    [Consumes("multipart/form-data")]
    public class UpdateFactoryImageEndPoint(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPut]
        public async Task<IActionResult> Update([FromForm] UpdateFactoryImageRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request, cancellationToken);

            return result.ToResponse();
        }
    }
}
