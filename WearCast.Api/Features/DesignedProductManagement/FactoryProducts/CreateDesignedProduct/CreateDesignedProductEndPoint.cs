namespace WearCast.Api.Features.DesignedProductManagement.FactoryProducts.CreateDesignedProduct
{
    [Route("api/factory/products")]
    [ApiController]
    public class CreateDesignedProductEndPoint(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDesignedProductRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request, cancellationToken);

            return result.ToResponse();
        }
    }
}
