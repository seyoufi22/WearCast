namespace WearCast.Api.Features.ShippingCompanies.GetWallet;

[Route("api/shipping-companies")]
[ApiController]
[Authorize(Roles = $"{DefaultRoles.ShippingCompanyManager},{DefaultRoles.SuperAdmin},{DefaultRoles.OperationsAdmin}")]
[Tags("Shipping Company Profile")]
public class GetShippingCompanyWalletEndpoint(ISender sender) : ControllerBase
{
    [HttpGet("wallet")]
    public async Task<IActionResult> GetWallet([FromQuery] int? id, CancellationToken cancellationToken)
    {
        var isAdmin = User.IsInRole(DefaultRoles.SuperAdmin) || User.IsInRole(DefaultRoles.OperationsAdmin);
        var result = await sender.Send(new GetShippingCompanyWalletRequest { AdminOverrideId = isAdmin ? id : null }, cancellationToken);
        return result.ToResponse();
    }
}
