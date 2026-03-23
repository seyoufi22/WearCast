namespace WearCast.Api.Features.DesignedProductManagement.CustomerDesigns.GetCustomerDesign
{
    [Route("api/customers/me/designs")]
    [ApiController]
    [Authorize(Roles = DefaultRoles.Customer)]
    public class GetCustomerDesignEndPoint(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        [HttpGet("{Id:int}")]
        public async Task<IActionResult> Get([FromRoute] int Id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetCustomerDesignRequest(Id), cancellationToken);

            return result.ToResponse();
        }
    }
}
