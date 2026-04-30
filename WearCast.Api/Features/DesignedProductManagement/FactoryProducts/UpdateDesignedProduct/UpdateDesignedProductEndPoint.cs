namespace WearCast.Api.Features.DesignedProductManagement.FactoryProducts.UpdateDesignedProduct
{
    [Route("api/factories/products")]
    [ApiController]
    [Authorize(Roles = $"{DefaultRoles.FactoryManager},{DefaultRoles.CatalogAdmin},{DefaultRoles.SuperAdmin}")]
    [Tags("Factory Product")]
    public class UpdateDesignedProductEndPoint(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        [Authorize]
        [HttpPut("{Id}")]
        public async Task<IActionResult> Update([FromRoute] int Id, [FromBody] UpdateProductBody body, CancellationToken cancellationToken)
        {
            var request = new UpdateDesignedProductRequest(
                Id,
                body.Name,
                body.Description,
                body.TargetAudiences,
                body.DressStyle,
                body.Price,
                body.CanvasWidth,
                body.CanvasHeight,
                body.CategoryId,
                body.DefaultColorId
                );
            var result = await mediator.Send(request, cancellationToken);

            return result.ToResponse();
        }
    }
    public record UpdateProductBody(
        string Name,
        string Description,
        List<TargetAudience> TargetAudiences,
        DressStyle DressStyle,
        decimal Price,
        int CanvasWidth,
        int CanvasHeight,
        int CategoryId,
        int? DefaultColorId
        );
}
