using WearCast.Api.Common.Enums;
using WearCast.Api.Features.Common.DTOs;

namespace WearCast.Api.Features.Customers.GetWallet;

public class GetCustomerWalletHandler(
    ApplicationDbContext context,
    IHttpContextAccessor httpContextAccessor
) : IRequestHandler<GetCustomerWalletRequest, Result<WalletResponse>>
{
    public async Task<Result<WalletResponse>> Handle(GetCustomerWalletRequest request, CancellationToken cancellationToken)
    {
        var user = httpContextAccessor.HttpContext!.User;
        var customerId = user.GetCustomerId();

        if (customerId == null)
            return Result.Failure<WalletResponse>(new Error("Customer.NotFound", "Customer not found in token.", StatusCodes.Status404NotFound));

        var wallet = await context.Wallets
            .AsNoTracking()
            .Where(w => w.OwnerType == WalletOwnerType.Customer && w.OwnerId == customerId.Value)
            .FirstOrDefaultAsync(cancellationToken);

        if (wallet == null)
        {
            return Result.Success(new WalletResponse(0, 0m, new List<WalletTransactionDto>()));
        }

        // For customers: sender is the seller or factory they purchased from
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
                        .Select(o => o.Seller != null ? o.Seller.Name : o.Factory != null ? o.Factory.Name : null)
                        .FirstOrDefault()
                    : null,
                t.ReferenceOrderId != null
                    ? context.Orders
                        .Where(o => o.Id == t.ReferenceOrderId)
                        .Select(o => o.Seller != null ? o.Seller.Email : o.Factory != null ? o.Factory.Email : null)
                        .FirstOrDefault()
                    : null,
                t.CreatedOn
            ))
            .ToListAsync(cancellationToken);

        return Result.Success(new WalletResponse(wallet.Id, wallet.Balance, transactions));
    }
}
