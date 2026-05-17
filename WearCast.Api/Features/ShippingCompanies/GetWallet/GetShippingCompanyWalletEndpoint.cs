namespace WearCast.Api.Features.ShippingCompanies.GetWallet;

[Route("api/shipping-companies")]
[ApiController]
[Authorize(Roles = $"{DefaultRoles.ShippingCompanyManager},{DefaultRoles.SuperAdmin},{DefaultRoles.OperationsAdmin}")]
[Tags("Shipping Company Profile")]
public class GetShippingCompanyWalletEndpoint(ISender sender) : ControllerBase
{
    [HttpGet("wallet")]
    public async Task<IActionResult> GetWallet(CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetShippingCompanyWalletRequest(), cancellationToken);
        return result.ToResponse();
    }
}
