namespace WearCast.Api.Features.DesignedProductManagement.FactoryProductImages.DeleteFactoryProductImage
{
    [ApiController]
    [Route("api/factory/product-images")]
    public class DeleteFactoryProductImageController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpDelete("{imageId:int}")]
        public async Task<IActionResult> DeleteImage(int imageId, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new DeleteFactoryProductImageRequest(imageId), cancellationToken);

            return result.ToResponse();
        }
    }
}
