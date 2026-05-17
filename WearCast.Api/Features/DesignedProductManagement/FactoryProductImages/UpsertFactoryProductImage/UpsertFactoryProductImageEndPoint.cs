
namespace WearCast.Api.Features.DesignedProductManagement.FactoryProductImages.UpsertFactoryProductImage
{
    [Route("api/factories/designed-product-colors/{colorId:int}/images")]
    [ApiController]
    [Tags("Factory Product Image")]
    [Authorize(Roles = $"{DefaultRoles.FactoryManager},{DefaultRoles.CatalogAdmin},{DefaultRoles.SuperAdmin}")]
    public class UpsertFactoryProductImageEndPoint(IMediator mediator) : ControllerBase
    {
        [HttpPut("{ViewSide}")]
        public async Task<IActionResult> UpdateImage(
            [FromRoute] int colorId,
            [FromRoute] ViewSide ViewSide,
            [FromForm] IFormFile NewImage,
            CancellationToken cancellationToken = default)
        {
            var request = new UpsertFactoryProductImageRequest(colorId, ViewSide, NewImage);
            var result = await mediator.Send(request, cancellationToken);

            return result.ToResponse();
        }
    }
}
