using WearCast.Api.Common.Enums;
using WearCast.Api.Entities.Wallet;

namespace WearCast.Api.Common.Wallet;

public interface IWalletService
{
    /// <summary>
    /// Thread-safe credit: atomically adds amount to wallet balance and logs a transaction.
    /// </summary>
    Task<WalletTransaction> CreditAsync(WalletOwnerType ownerType, int ownerId, decimal amount, string description, int? referenceOrderId = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Thread-safe debit: atomically subtracts amount from wallet balance and logs a transaction.
    /// Fails if insufficient balance.
    /// </summary>
    Task<Result<WalletTransaction>> DebitAsync(WalletOwnerType ownerType, int ownerId, decimal amount, string description, int? referenceOrderId = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets or creates a wallet for the given owner.
    /// </summary>
    Task<Entities.Wallet.Wallet> GetOrCreateWalletAsync(WalletOwnerType ownerType, int ownerId, CancellationToken cancellationToken = default);
}
