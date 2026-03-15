namespace WearCast.Api.Features.DesignedProductManagement.CustomerUploadedImages.DeleteCustomerUploadedImages
{
    [Route("api/customers/me/design-images")]
    [ApiController]
    [Authorize]
    public class DeleteCustomerUploadedImagesEndPoint(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new DeleteCustomerUploadedImagesRequest(id), cancellationToken);

            return result.ToResponse();
        }
    }
}
