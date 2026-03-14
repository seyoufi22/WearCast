namespace WearCast.Api.Features.DesignedProductManagement.FactoryProductImages.DeleteFactoryProductImage
{
    [Route("api/factory/product-colors")]
    [ApiController]
    public class DeleteFactoryProductImageEndPoint(IMediator mediator) : ControllerBase
    {
        [HttpDelete("{colorSlug}/images/{imageId}")]
        public async Task<IActionResult> Delete(
            [FromRoute] string colorSlug,
            [FromRoute] int imageId,
            CancellationToken cancellationToken)
        {
            var request = new DeleteFactoryProductImageRequest(colorSlug, imageId);
            var result = await mediator.Send(request, cancellationToken);

            return result.ToResponse();
        }
    }
}
