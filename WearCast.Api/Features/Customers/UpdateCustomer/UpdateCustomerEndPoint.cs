namespace WearCast.Api.Features.Customers.UpdateCustomer
{
    [Route("api/customers")]
    [ApiController]
    [Authorize(Roles = $"{DefaultRoles.Customer},{DefaultRoles.CustomerServiceAdmin},{DefaultRoles.SuperAdmin}")]
    [Tags("Customer Profile")]
    public class UpdateCustomerEndPoint(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateCustomerRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request, cancellationToken);

            return result.ToResponse();
        }

    }
}
