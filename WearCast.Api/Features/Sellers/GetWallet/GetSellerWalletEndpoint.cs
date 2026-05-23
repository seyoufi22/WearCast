namespace WearCast.Api.Features.Sellers.GetWallet;

[Route("api/sellers")]
[ApiController]
[Authorize(Roles = $"{DefaultRoles.SellerManager},{DefaultRoles.SuperAdmin},{DefaultRoles.VendorAdmin}")]
[Tags("Seller Profile")]
public class GetSellerWalletEndpoint(ISender sender) : ControllerBase
{
    [HttpGet("wallet")]
    public async Task<IActionResult> GetWallet([FromQuery] int? id, CancellationToken cancellationToken)
    {
        var isAdmin = User.IsInRole(DefaultRoles.SuperAdmin) || User.IsInRole(DefaultRoles.VendorAdmin);
        var result = await sender.Send(new GetSellerWalletRequest { AdminOverrideId = isAdmin ? id : null }, cancellationToken);
        return result.ToResponse();
    }
}
