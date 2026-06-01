namespace WearCast.Api.Features.Customers.GetWallet;

[Route("api/customers")]
[ApiController]
[Authorize(Roles = $"{DefaultRoles.Customer},{DefaultRoles.SuperAdmin},{DefaultRoles.CustomerServiceAdmin}")]
[Tags("Customer Profile")]
public class GetCustomerWalletEndpoint(ISender sender) : ControllerBase
{
    [HttpGet("wallet")]
    public async Task<IActionResult> GetWallet([FromQuery] int? id, CancellationToken cancellationToken)
    {
        var isAdmin = User.IsInRole(DefaultRoles.SuperAdmin) || User.IsInRole(DefaultRoles.CustomerServiceAdmin);
        var result = await sender.Send(new GetCustomerWalletRequest { AdminOverrideId = isAdmin ? id : null }, cancellationToken);
        return result.ToResponse();
    }
}
