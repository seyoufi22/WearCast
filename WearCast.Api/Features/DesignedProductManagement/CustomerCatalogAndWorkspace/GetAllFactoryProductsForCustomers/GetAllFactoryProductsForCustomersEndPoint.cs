namespace WearCast.Api.Features.DesignedProductManagement.CustomerCatalogAndWorkspace.GetAllFactoryProductsForCustomers
{
    [Route("api/customer/catalog/designed-products")]
    [ApiController]
    [Tags("Designed Product Catalog")]
    public class GetAllFactoryProductsForCustomersEndPoint(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetAllFactoryProductsForCustomersRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request, cancellationToken);
            return result.ToResponse();
        }
    }
}
