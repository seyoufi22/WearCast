namespace WearCast.Api.Features.DesignedProductManagement.CustomerUploadedImages.UpdateCustomerUploadedImage
{
    [Route("api/customers/me/design-images")]
    [ApiController]
    [Authorize(Roles = DefaultRoles.Customer)]
    [Tags("Customer Uploaded Images")]
    public class UpdateCustomerUploadedImageEndPoint(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPut("{Id:int}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Update([FromRoute] int Id, [FromForm] UpdateCustomerUploadedImageForm form, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new UpdateCustomerUploadedImageRequest(Id, form.Image), cancellationToken);

            return result.ToResponse();
        }
        public record UpdateCustomerUploadedImageForm(IFormFile Image);
    }
}
