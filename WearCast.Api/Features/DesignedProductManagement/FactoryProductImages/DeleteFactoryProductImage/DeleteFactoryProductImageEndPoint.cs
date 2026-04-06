namespace WearCast.Api.Features.DesignedProductManagement.FactoryProductImages.DeleteFactoryProductImage
{
    [ApiController]
    [Route("api/factories/product-images")]
    [Tags("Factory Product Image")]
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
