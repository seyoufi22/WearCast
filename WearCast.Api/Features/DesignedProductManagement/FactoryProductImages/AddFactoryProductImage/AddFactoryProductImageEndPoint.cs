namespace WearCast.Api.Features.DesignedProductManagement.FactoryProductImages.AddFactoryProductImage
{
    [Route("api/factory/product-colors")]
    [ApiController]
    public class AddFactoryProductImageEndPoint(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost("{colorSlug}/images")]
        public async Task<IActionResult> Add(
            [FromRoute] string colorSlug,
            [FromForm] AddFactoryProductImageForm form,
            CancellationToken cancellationToken)
        {
            var request = new AddFactoryProductImageRequest(colorSlug, form.Image, form.ViewSide);
            var result = await _mediator.Send(request, cancellationToken);

            return result.ToResponse();
        }
    }
    public record AddFactoryProductImageForm(IFormFile Image, ViewSide ViewSide);

}
