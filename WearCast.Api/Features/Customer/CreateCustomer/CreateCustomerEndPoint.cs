
namespace WearCast.Api.Features.Customer.CreateCustomer;

[Route("CreateCustomer")]
[ApiController]
public class CreateCustomerEndPoint(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost]
    public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        return result.ToResponse();
    }
}