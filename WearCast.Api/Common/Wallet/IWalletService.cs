using WearCast.Api.Common.Enums;
using WearCast.Api.Entities.Wallet;

namespace WearCast.Api.Common.Wallet;

public interface IWalletService
{
    Task<WalletTransaction> CreditAsync(WalletOwnerType ownerType, int ownerId, decimal amount, string description, int? referenceOrderId = null, string? performedById = null, CancellationToken cancellationToken = default);

    Task<Result<WalletTransaction>> DebitAsync(WalletOwnerType ownerType, int ownerId, decimal amount, string description, int? referenceOrderId = null, string? performedById = null, CancellationToken cancellationToken = default);

    Task<Entities.Wallet.Wallet> GetOrCreateWalletAsync(WalletOwnerType ownerType, int ownerId, string? performedById = null, CancellationToken cancellationToken = default);
}
