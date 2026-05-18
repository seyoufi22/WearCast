namespace WearCast.Api.Features.Factories.GetFactory;

[Route("api/factories")]
[ApiController]
[Authorize(Roles = $"{DefaultRoles.FactoryManager},{DefaultRoles.SuperAdmin},{DefaultRoles.VendorAdmin}")]
[Tags("Factory Profile")]
public class GetFactoryEndPoint(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("profile")]
    public async Task<IActionResult> GetProfile(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetFactoryRequest(), cancellationToken);

        return result.ToResponse();
    }
}