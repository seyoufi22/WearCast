using System.Security.Claims;
namespace WearCast.Api.Features.Factories.FactoryManagers.DeleteFactoryManager;

[Route("api/factories/managers")]
[ApiController]
[Authorize(Roles = $"{DefaultRoles.FactoryManager},{DefaultRoles.SuperAdmin},{DefaultRoles.VendorAdmin}")]
[Tags("Factory Managers")]
public class DeleteFactoryManagerEndpoint(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpDelete("{factoryManagerId:int}")]
    public async Task<IActionResult> DeleteFactoryManager(
        [FromRoute] int factoryManagerId,
        [FromBody] DeleteFactoryManagerBody body,
        CancellationToken cancellationToken)
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(currentUserId))
            return Unauthorized();

        bool isAdmin = User.IsInRole(DefaultRoles.SuperAdmin) || User.IsInRole(DefaultRoles.VendorAdmin);

        var request = new DeleteFactoryManagerRequest(factoryManagerId, currentUserId, isAdmin, body.Reason);
        var result = await _mediator.Send(request, cancellationToken);

        return result.ToResponse();
    }
}