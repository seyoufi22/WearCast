namespace WearCast.Api.Features.Factories.GetWallet;

[Route("api/factories")]
[ApiController]
[Authorize(Roles = $"{DefaultRoles.FactoryManager},{DefaultRoles.SuperAdmin}")]
[Tags("Factory Profile")]
public class GetFactoryWalletEndpoint(ISender sender) : ControllerBase
{
    [HttpGet("wallet")]
    public async Task<IActionResult> GetWallet(CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetFactoryWalletRequest(), cancellationToken);
        return result.ToResponse();
    }
}
