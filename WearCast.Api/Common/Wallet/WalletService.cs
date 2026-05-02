using Microsoft.EntityFrameworkCore;
using WearCast.Api.Abstractions;
using WearCast.Api.Common.Enums;
using WearCast.Api.Entities.Wallet;
using WearCast.Api.Persistence;

namespace WearCast.Api.Common.Wallet;

public class WalletService(ApplicationDbContext dbContext) : IWalletService
{
    public async Task<Entities.Wallet.Wallet> GetOrCreateWalletAsync(WalletOwnerType ownerType, int ownerId, CancellationToken cancellationToken = default)
    {
        var wallet = await dbContext.Wallets
            .FirstOrDefaultAsync(w => w.OwnerType == ownerType && w.OwnerId == ownerId, cancellationToken);

        if (wallet == null)
        {
            wallet = new Entities.Wallet.Wallet
            {
                OwnerType = ownerType,
                OwnerId = ownerId,
                Balance = 0m
            };
            dbContext.Wallets.Add(wallet);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        return wallet;
    }

    public async Task<WalletTransaction> CreditAsync(WalletOwnerType ownerType, int ownerId, decimal amount, string description, int? referenceOrderId = null, CancellationToken cancellationToken = default)
    {
        var wallet = await GetOrCreateWalletAsync(ownerType, ownerId, cancellationToken);

        // Thread-safe atomic increment at DB level: UPDATE Wallets SET Balance = Balance + @amount WHERE Id = @id
        await dbContext.Wallets
            .Where(w => w.Id == wallet.Id)
            .ExecuteUpdateAsync(setters => setters.SetProperty(w => w.Balance, w => w.Balance + amount), cancellationToken);

        // Reload to get the updated balance
        await dbContext.Entry(wallet).ReloadAsync(cancellationToken);

        var transaction = new WalletTransaction
        {
            WalletId = wallet.Id,
            Type = TransactionType.Credit,
            Amount = amount,
            BalanceAfter = wallet.Balance,
            Description = description,
            ReferenceOrderId = referenceOrderId
        };

        dbContext.WalletTransactions.Add(transaction);
        await dbContext.SaveChangesAsync(cancellationToken);

        return transaction;
    }

    public async Task<Result<WalletTransaction>> DebitAsync(WalletOwnerType ownerType, int ownerId, decimal amount, string description, int? referenceOrderId = null, CancellationToken cancellationToken = default)
    {
        var wallet = await GetOrCreateWalletAsync(ownerType, ownerId, cancellationToken);

        if (wallet.Balance < amount)
        {
            return Result.Failure<WalletTransaction>(
                new Error("Wallet.InsufficientBalance", $"Insufficient balance. Current: {wallet.Balance}, Requested: {amount}", Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest));
        }

        // Thread-safe atomic decrement at DB level with balance check
        var affected = await dbContext.Wallets
            .Where(w => w.Id == wallet.Id && w.Balance >= amount)
            .ExecuteUpdateAsync(setters => setters.SetProperty(w => w.Balance, w => w.Balance - amount), cancellationToken);

        if (affected == 0)
        {
            return Result.Failure<WalletTransaction>(
                new Error("Wallet.InsufficientBalance", $"Insufficient balance for debit of {amount}.", Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest));
        }

        // Reload to get the updated balance
        await dbContext.Entry(wallet).ReloadAsync(cancellationToken);

        var transaction = new WalletTransaction
        {
            WalletId = wallet.Id,
            Type = TransactionType.Debit,
            Amount = amount,
            BalanceAfter = wallet.Balance,
            Description = description,
            ReferenceOrderId = referenceOrderId
        };

        dbContext.WalletTransactions.Add(transaction);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result<WalletTransaction>.Success(transaction);
    }
}
