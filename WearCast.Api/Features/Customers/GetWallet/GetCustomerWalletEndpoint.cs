namespace WearCast.Api.Features.Customers.GetWallet;

[Route("api/customers")]
[ApiController]
[Authorize(Roles = $"{DefaultRoles.Customer},{DefaultRoles.SuperAdmin},{DefaultRoles.CustomerServiceAdmin}")]
[Tags("Customer Profile")]
public class GetCustomerWalletEndpoint(ISender sender) : ControllerBase
{
    [HttpGet("wallet")]
    public async Task<IActionResult> GetWallet(CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetCustomerWalletRequest(), cancellationToken);
        return result.ToResponse();
    }
}
