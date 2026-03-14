namespace WearCast.Api.Features.DesignedProductManagement.FactoryProductSizes.AddFactoryProductSize
{
    [Route("api/factory/products")]
    [ApiController]
    public class AddFactoryProductSizeEndPoint(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        [HttpPost("{productSlug}/sizes")]
        public async Task<IActionResult> Add([FromRoute] string productSlug, [FromBody] AddFactoryProductSizeBody body, CancellationToken cancellationToken)
        {
            var request = new AddFactoryProductSizeRequest(productSlug, body.Size, body.A, body.B, body.C);

            var result = await mediator.Send(request, cancellationToken);

            return result.ToResponse();
        }
        public record AddFactoryProductSizeBody(Size Size, decimal? A, decimal? B, decimal? C);
    }
}
