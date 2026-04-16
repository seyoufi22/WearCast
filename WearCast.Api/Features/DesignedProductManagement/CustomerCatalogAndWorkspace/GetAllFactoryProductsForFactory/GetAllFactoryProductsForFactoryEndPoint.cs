namespace WearCast.Api.Features.DesignedProductManagement.CustomerCatalogAndWorkspace.GetAllFactoryProductsForFactory
{
    [Route("api/factories/catalog/designed-products")]
    [ApiController]
    // [Authorize(Roles = $"{DefaultRoles.FactoryManager},{DefaultRoles.SuperAdmin}")]
    [Tags("Designed Product Catalog")]
    public class GetAllFactoryProductsForFactoryEndPoint(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetAllFactoryProductsForFactoryRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request, cancellationToken);
            return result.ToResponse();
        }
    }
}
