namespace WearCast.Api.Features.DesignedProductManagement.FactoryProductColors.UpdateFactoryProductColor
{
    [Route("api/factories/products")]
    [ApiController]
    [Tags("Factory Product Color")]
    public class UpdateFactoryProductColorEndPoint(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        [Authorize]
        [HttpPut("{productId}/colors/{colorId}")]
        public async Task<IActionResult> Update(
            [FromRoute] int productId,
            [FromRoute] int colorId,
            [FromBody] UpdateFactoryProductColorBody body,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new UpdateFactoryProductColorRequest(
                colorId,
                body.Name,
                body.HexCode,
                productId), cancellationToken);

            return result.ToResponse();

        }
        public record UpdateFactoryProductColorBody(string Name, string HexCode);
    }
}
