namespace WearCast.Api.Features.DesignedProductManagement.CustomerCatalogAndWorkspace.GetProductDetails
{
    [ApiController]
    [Route("api/catalog/designed-products")]
    public class GetProductDetailsEndPoints(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Get([FromRoute] int Id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetProductDetailsRequest(Id), cancellationToken);

            return result.ToResponse();
        }
    }
}
