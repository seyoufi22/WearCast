namespace WearCast.Api.Features.Sellers.GetWallet;

[Route("api/sellers")]
[ApiController]
[Authorize(Roles = $"{DefaultRoles.SellerManager},{DefaultRoles.SuperAdmin}")]
[Tags("Seller Profile")]
public class GetSellerWalletEndpoint(ISender sender) : ControllerBase
{
    [HttpGet("wallet")]
    public async Task<IActionResult> GetWallet(CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetSellerWalletRequest(), cancellationToken);
        return result.ToResponse();
    }
}
