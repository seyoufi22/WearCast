using WearCast.Api.Features.Common.DTOs;

namespace WearCast.Api.Features.Customers.GetWallet;

public class GetCustomerWalletRequest : IRequest<Result<WalletResponse>>
{
    public int? AdminOverrideId { get; init; }
}
