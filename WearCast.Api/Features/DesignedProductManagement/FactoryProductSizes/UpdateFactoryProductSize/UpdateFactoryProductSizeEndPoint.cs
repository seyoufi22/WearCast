namespace WearCast.Api.Features.DesignedProductManagement.FactoryProductSizes.UpdateFactoryProductSize
{
    [Route("api/factory/products")]
    [ApiController]
    public class UpdateFactoryProductSizeEndPoint(IMediator mediator) : ControllerBase
    {
        [HttpPut("{productSlug}/sizes/{size}")]
        public async Task<IActionResult> Update(
            [FromRoute] string productSlug,
            [FromRoute] Size size,
            [FromBody] UpdateFactoryProductSizeBody body,
            CancellationToken cancellationToken)
        {
            var request = new UpdateFactoryProductSizeRequest(productSlug, size, body.A, body.B, body.C);
            var result = await mediator.Send(request, cancellationToken);

            return result.ToResponse();
        }
    }
    public record UpdateFactoryProductSizeBody(decimal? A, decimal? B, decimal? C);
}
