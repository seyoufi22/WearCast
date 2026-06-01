namespace WearCast.Api.Features.Factories.GetWallet;

[Route("api/factories")]
[ApiController]
[Authorize(Roles = $"{DefaultRoles.FactoryManager},{DefaultRoles.SuperAdmin},{DefaultRoles.VendorAdmin}")]
[Tags("Factory Profile")]
public class GetFactoryWalletEndpoint(ISender sender) : ControllerBase
{
    [HttpGet("wallet")]
    public async Task<IActionResult> GetWallet([FromQuery] int? id, CancellationToken cancellationToken)
    {
        var isAdmin = User.IsInRole(DefaultRoles.SuperAdmin) || User.IsInRole(DefaultRoles.VendorAdmin);
        var result = await sender.Send(new GetFactoryWalletRequest { AdminOverrideId = isAdmin ? id : null }, cancellationToken);
        return result.ToResponse();
    }
}
