namespace WearCast.Api.Features.ShippingCompanies.ShippingCompanyManagers.GetShippingCompanyManager;

[ApiController]
[Route("api/shipping-company-managers")]
[Authorize(Roles = $"{DefaultRoles.ShippingCompanyManager},{DefaultRoles.SuperAdmin},{DefaultRoles.OperationsAdmin}")]
[Tags("Shipping Company Manager Profile")]
public class GetShippingCompanyManagerEndPoint(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("profile")]
    public async Task<IActionResult> GetProfile([FromQuery] GetShippingCompanyManagerRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);

        return result.ToResponse();
    }
}