namespace WearCast.Api.Features.DesignedProductManagement.FactoryProductSizes.DeleteFactoryProductSize
{
    [Route("api/factory/products")]
    [ApiController]
    public class DeleteFactoryProductSizeEndPoint(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpDelete("{productSlug}/sizes/{size}")]
        public async Task<IActionResult> Delete(
            [FromRoute] string productSlug,
            [FromRoute] Size size,
            CancellationToken cancellationToken)
        {
            var request = new DeleteFactoryProductSizeRequest(productSlug, size);
            var result = await mediator.Send(request, cancellationToken);

            return result.ToResponse();
        }
    }
}
