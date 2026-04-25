namespace WearCast.Api.Features.Customers.GetAllCustomers;

[Route("api/admin/customers")]
[ApiController]
[Authorize(Roles = DefaultRoles.SuperAdmin)]
[Tags("Customer Profile")]
public class CustomersManagementEndpoint(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("all")] 
    public async Task<IActionResult> GetAllCustomers([FromQuery] GetAllCustomersRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        return result.ToResponse();
    }
}