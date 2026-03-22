namespace WearCast.Api.Features.DesignedProductManagement.FactoryProductSizes.AddFactoryProductSize
{
    [Route("api/factory/products")]
    [ApiController]
    public class AddFactoryProductSizeEndPoint(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        [HttpPost("{productId}/sizes")]
        public async Task<IActionResult> Add([FromRoute] int productId, [FromBody] AddFactoryProductSizeBody body, CancellationToken cancellationToken)
        {
            var request = new AddFactoryProductSizeRequest(body.Size, body.A, body.B, body.C, productId);

            var result = await mediator.Send(request, cancellationToken);

            return result.ToResponse();
        }
        public record AddFactoryProductSizeBody(Size Size, decimal? A, decimal? B, decimal? C);
    }
}
