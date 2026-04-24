namespace WearCast.Api.Features.DesignedProductManagement.FactoryProductColors.AddFactoryProductColor
{
    [Route("api/factories/products")]
    [ApiController]
    [Tags("Factory Product Color")]
    public class AddFactoryProductColorEndPoint(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        [Authorize]
        [HttpPost("{productId}/colors")]
        public async Task<IActionResult> Add([FromRoute] int productId, [FromForm] CreateFactoryProductColorBForm form, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new AddFactoryProductColorRequest(form.Name, form.HexCode, form.Image, productId), cancellationToken);

            return result.ToResponse();
        }


        public record CreateFactoryProductColorBForm(string Name, string HexCode, IFormFile Image);
    }
}
