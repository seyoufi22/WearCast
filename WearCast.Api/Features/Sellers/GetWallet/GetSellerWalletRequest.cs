using WearCast.Api.Features.Common.DTOs;

namespace WearCast.Api.Features.Sellers.GetWallet;

public class GetSellerWalletRequest : IRequest<Result<WalletResponse>>
{
    public int? AdminOverrideId { get; init; }
}
