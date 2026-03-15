namespace WearCast.Api.Features.DesignedProductManagement.CustomerUploadedImages.AddCustomerUploadedImage
{
    [Route("api/customers/me/design-images")]
    [ApiController]
    [Authorize]
    public class AddCustomerUploadedImageEndPoint(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Add([FromForm] AddCustomerUploadedImageRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request, cancellationToken);

            return result.ToResponse();
        }
    }
}
