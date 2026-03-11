namespace WearCast.Api.Features.DesignedProductManagement.FactoryProductColors.AddFactoryProductColor
{
    [Route("api/factory/products")]
    [ApiController]
    public class AddFactoryProductColorEndPoint(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        [Authorize]
        [HttpPost("{productSlug}/colors")]
        public async Task<IActionResult> Add([FromRoute] string productSlug, [FromBody] CreateFactoryProductColorBody body, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new AddFactoryProductColorRequest(productSlug, body.Name, body.HexCode), cancellationToken);

            return result.ToResponse();
        }


        public record CreateFactoryProductColorBody(string Name, string HexCode);
    }
}
