namespace WearCast.Api.Features.DesignedProductManagement.CustomerDesigns.GetAllCustomerDesigns
{
    [Route("api/customers/me/designs")]
    [ApiController]
    [Authorize(Roles = DefaultRoles.Customer)]
    [Tags("Customer Design")]
    public class GetAllCustomerDesignsEndPoint(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? searchTerm = null,
            [FromQuery] int pageIndex = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            var request = new GetAllCustomerDesignsRequest(searchTerm, pageIndex, pageSize);

            var result = await _mediator.Send(request, cancellationToken);

            return result.ToResponse();
        }
    }
}