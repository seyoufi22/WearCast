namespace WearCast.Api.Features.DesignedProductManagement.FactoryProductImages.UpdateFactoryProductImage
{
    [Route("api/factory/product-colors")]
    [ApiController]
    public class UpdateFactoryProductImageEndPoint(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        [HttpPut("{colorSlug}/images/{imageId}")]
        public async Task<IActionResult> Update(
            [FromRoute] string colorSlug,
            [FromRoute] int imageId,
            [FromForm] UpdateDesignedProductImageForm form,
            CancellationToken cancellationToken)
        {
            var request = new UpdateFactoryProductImageRequest(colorSlug, imageId, form.Image, form.ViewSide);
            var result = await mediator.Send(request, cancellationToken);

            return result.ToResponse();
        }
    }
    public record UpdateDesignedProductImageForm(IFormFile? Image, ViewSide ViewSide);
}
