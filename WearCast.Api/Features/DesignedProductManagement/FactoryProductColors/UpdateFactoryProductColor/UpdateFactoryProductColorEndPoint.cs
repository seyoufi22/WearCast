namespace WearCast.Api.Features.DesignedProductManagement.FactoryProductColors.UpdateFactoryProductColor
{
    [Route("api/factory/products")]
    [ApiController]
    public class UpdateFactoryProductColorEndPoint(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        [Authorize]
        [HttpPut("{productSlug}/colors/{colorSlug}")]
        public async Task<IActionResult> Update(
            [FromRoute] string productSlug,
            [FromRoute] string colorSlug,
            [FromBody] UpdateFactoryProductColorBody body,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new UpdateFactoryProductColorRequest(
                productSlug,
                colorSlug,
                body.Name,
                body.HexCode), cancellationToken);

            return result.ToResponse();

        }
        public record UpdateFactoryProductColorBody(string Name, string HexCode);
    }
}
