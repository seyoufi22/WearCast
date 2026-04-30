

namespace WearCast.Api.Features.DesignedProductManagement.FactoryProductColors.UpdateFactoryProductColorMainImage
{
    [Route("api/factories/products")]
    [ApiController]
    [Tags("Factory Product Color")]
    [Authorize(Roles = $"{DefaultRoles.FactoryManager},{DefaultRoles.CatalogAdmin},{DefaultRoles.SuperAdmin}")]
    public class UpdateFactoryProductColorMainImageEndPoint(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        [Authorize]
        [HttpPut("{productId}/colors/{colorId}/main-image")]
        public async Task<IActionResult> Update([FromRoute] int productId, [FromRoute] int colorId,
            [FromForm] UpdateFactoryProductColorMainImageForm form, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                new UpdateFactoryProductColorMainImageRequest(
                form.Image,
                productId,
                colorId),
                cancellationToken);

            return result.ToResponse();
        }
        public record UpdateFactoryProductColorMainImageForm(IFormFile Image);
    }
}
