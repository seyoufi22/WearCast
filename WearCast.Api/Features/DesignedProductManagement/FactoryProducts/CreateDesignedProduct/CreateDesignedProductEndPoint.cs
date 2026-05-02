namespace WearCast.Api.Features.DesignedProductManagement.FactoryProducts.CreateDesignedProduct
{
    [Route("api/factories/products")]
    [ApiController]
    [Tags("Factory Product")]
    [Authorize(Roles = $"{DefaultRoles.FactoryManager},{DefaultRoles.CatalogAdmin},{DefaultRoles.SuperAdmin}")]
    public class CreateDesignedProductEndPoint(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CreateDesignedProductRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request, cancellationToken);

            return result.ToResponse();
        }
    }
}
