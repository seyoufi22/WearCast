namespace WearCast.Api.Features.DesignedProductManagement.FactoryProductImages.UpdateFactoryProductImage
{
    [Route("api/factories/product-images")]
    [ApiController]
    [Tags("Factory Product Image")]
    public class UpdateFactoryProductImageEndPoint(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPut("{imageId:int}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Update(
            [FromRoute] int imageId,
            [FromForm] UpdateDesignedProductImageForm form,
            CancellationToken cancellationToken)
        {
            var request = new UpdateFactoryProductImageRequest(imageId, form.Image, form.ViewSide);

            var result = await _mediator.Send(request, cancellationToken);

            return result.ToResponse();
        }
    }
    public record UpdateDesignedProductImageForm(IFormFile? Image, ViewSide ViewSide);
}
