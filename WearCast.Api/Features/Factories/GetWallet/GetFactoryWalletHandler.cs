using WearCast.Api.Common.Enums;
using WearCast.Api.Features.Common.DTOs;

namespace WearCast.Api.Features.Factories.GetWallet;

public class GetFactoryWalletHandler(
    ApplicationDbContext context,
    IHttpContextAccessor httpContextAccessor
) : IRequestHandler<GetFactoryWalletRequest, Result<WalletResponse>>
{
    public async Task<Result<WalletResponse>> Handle(GetFactoryWalletRequest request, CancellationToken cancellationToken)
    {
        var user = httpContextAccessor.HttpContext!.User;
        var factoryId = user.GetFactoryId();

        if (factoryId == null)
            return Result.Failure<WalletResponse>(new Error("Factory.NotFound", "Factory not found in token.", StatusCodes.Status404NotFound));

        var wallet = await context.Wallets
            .AsNoTracking()
            .Where(w => w.OwnerType == WalletOwnerType.Factory && w.OwnerId == factoryId.Value)
            .FirstOrDefaultAsync(cancellationToken);

        if (wallet == null)
        {
            return Result.Success(new WalletResponse(0, 0m, new List<WalletTransactionDto>()));
        }

        // For factories: sender is the customer who placed the order
        var transactions = await context.WalletTransactions
            .AsNoTracking()
            .Where(t => t.WalletId == wallet.Id)
            .OrderByDescending(t => t.CreatedOn)
            .Select(t => new WalletTransactionDto(
                t.Id,
                t.Type.ToString(),
                t.Amount,
                t.BalanceAfter,
                t.Description,
                t.ReferenceOrderId,
                t.ReferenceOrderId != null
                    ? context.Orders
                        .Where(o => o.Id == t.ReferenceOrderId)
                        .Select(o => o.Customer.ApplicationUser!.FirstName + " " + o.Customer.ApplicationUser.LastName)
                        .FirstOrDefault()
                    : null,
                t.ReferenceOrderId != null
                    ? context.Orders
                        .Where(o => o.Id == t.ReferenceOrderId)
                        .Select(o => o.Customer.ApplicationUser!.Email)
                        .FirstOrDefault()
                    : null,
                t.CreatedOn
            ))
            .ToListAsync(cancellationToken);

        return Result.Success(new WalletResponse(wallet.Id, wallet.Balance, transactions));
    }
}
