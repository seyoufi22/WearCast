namespace WearCast.Api.Features.Customers.GetCustomer
{
    [Route("api/customers")]
    [ApiController]
    [Authorize(Roles = $"{DefaultRoles.Customer},{DefaultRoles.SuperAdmin},{DefaultRoles.CustomerServiceAdmin}")]
    [Tags("Customer Profile")]
    public class GetCustomerEndPoint(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile([FromQuery] GetCustomerRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request, cancellationToken);

            return result.ToResponse(); 
        }
    }
}