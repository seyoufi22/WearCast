namespace WearCast.Api.Features.DesignedProductManagement.CustomerCatalogAndWorkspace.GetProductDetails
{
    [Route("api/catalog/designed-products")]
    [ApiController]
    [Tags("Designed Product Catalog")]
    public class GetProductDetailsEndPoints(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get([FromRoute] int Id, [FromQuery] int? colorId, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetProductDetailsRequest(Id, colorId), cancellationToken);

            return result.ToResponse();
        }
    }
}
