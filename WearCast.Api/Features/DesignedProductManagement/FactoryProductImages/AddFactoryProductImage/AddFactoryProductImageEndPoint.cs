namespace WearCast.Api.Features.DesignedProductManagement.FactoryProductImages.AddFactoryProductImage
{
    [Route("api/factories/product-colors")]
    [ApiController]
    [Tags("Factory Product Image")]
    public class AddFactoryProductImageEndPoint(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost("{colorId}/images")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Add(
            [FromRoute] int colorId,
            [FromForm] AddFactoryProductImageForm form,
            CancellationToken cancellationToken)
        {
            var request = new AddFactoryProductImageRequest(colorId, form.Image, form.ViewSide);
            var result = await _mediator.Send(request, cancellationToken);

            return result.ToResponse();
        }
    }
    public record AddFactoryProductImageForm(IFormFile Image, ViewSide ViewSide);

}
