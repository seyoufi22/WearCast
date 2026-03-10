namespace WearCast.Api.Features.DesignedProductManagement.FactoryProducts.DeleteDesignedProduct
{
    [Route("api/factory/products")]
    [ApiController]
    public class DeleteDesignedProductEndPoint(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        [HttpDelete("{slug}")]
        public async Task<IActionResult> Delete([FromRoute] string slug, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new DeleteDesignedProductRequest(slug), cancellationToken);

            return result.ToResponse();
        }
    }
}
