using WearCast.Api.Features.Common.DTOs;

namespace WearCast.Api.Features.Factories.GetWallet;

public class GetFactoryWalletRequest : IRequest<Result<WalletResponse>>
{
    public int? AdminOverrideId { get; init; }
}
