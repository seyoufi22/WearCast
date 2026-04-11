namespace WearCast.Api.Features.ShippingCompanies.GetShippingCompany;

[Route("api/shipping-companies")]
[ApiController]
[Authorize(Roles = $"{DefaultRoles.ShippingCompanyManager},{DefaultRoles.SuperAdmin}")]
[Tags("Shipping Company Profile")]
public class GetShippingCompanyEndPoint(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("profile")]
    public async Task<IActionResult> GetProfile([FromQuery] GetShippingCompanyRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);

        return result.ToResponse();
    }
}