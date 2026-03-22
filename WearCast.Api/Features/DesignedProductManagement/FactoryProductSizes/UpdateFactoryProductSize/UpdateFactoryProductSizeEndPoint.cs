namespace WearCast.Api.Features.DesignedProductManagement.FactoryProductSizes.UpdateFactoryProductSize
{
    [Route("api/factory/product-sizes")]
    [ApiController]
    public class UpdateFactoryProductSizeEndPoint(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPut("{Id:int}")]
        public async Task<IActionResult> Update(
            [FromRoute] int Id,
            [FromBody] UpdateFactoryProductSizeBody body,
            CancellationToken cancellationToken)
        {
            var request = new UpdateFactoryProductSizeRequest(Id, body.A, body.B, body.C);

            var result = await _mediator.Send(request, cancellationToken);

            return result.ToResponse();
        }
    }
    public record UpdateFactoryProductSizeBody(decimal? A, decimal? B, decimal? C);
}
