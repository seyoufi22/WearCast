namespace WearCast.Api.Features.Factories.FactoryManagers.GetFactoryManager;

[Route("api/factory-managers")]
[ApiController]
[Authorize(Roles = $"{DefaultRoles.FactoryManager},{DefaultRoles.SuperAdmin},{DefaultRoles.VendorAdmin}")]
[Tags("Factory Manager Profile")]
public class GetFactoryManagerEndPoint(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("profile")]
    public async Task<IActionResult> GetProfile([FromQuery] GetFactoryManagerRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);

        return result.ToResponse();
    }
}