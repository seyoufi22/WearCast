namespace WearCast.Api.Features.Sellers.DeleteSeller;

using Microsoft.Extensions.Logging;
public class DeleteSellerHandler(
    ApplicationDbContext context,
    EmailHelper emailHelper,
    ILogger<DeleteSellerHandler> logger
) : IRequestHandler<DeleteSellerRequest, Result>
{
    private readonly ApplicationDbContext _context = context;
    private readonly EmailHelper _emailHelper = emailHelper;
    private readonly ILogger<DeleteSellerHandler> _logger = logger;

    public async Task<Result> Handle(DeleteSellerRequest request, CancellationToken cancellationToken)
    {
        var seller = await _context.Sellers
            .Where(s => s.Id == request.SellerId)
            .Select(s => new
            {
                Seller = s,
                ManagersInfo = s.Managers.Select(m => new
                {
                    Email = m.ApplicationUser!.Email,
                    FirstName = m.ApplicationUser.FirstName,
                    LastName = m.ApplicationUser.LastName
                }).ToList()
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (seller is null)
            return Result.Failure(SellerErrors.SellerNotFound);

        using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            await _context.FixedProductColors
                .Where(pc => _context.FixedProducts.Any(p => p.Id == pc.ProductId && p.SellerId == request.SellerId))
                .ExecuteUpdateAsync(setters => setters.SetProperty(x => x.IsDeleted, true), cancellationToken);

            await _context.FixedProducts
                .Where(p => p.SellerId == request.SellerId)
                .ExecuteUpdateAsync(setters => setters.SetProperty(x => x.IsDeleted, true), cancellationToken);

            await _context.Users
                .Where(u => _context.SellerManagers.Any(m => m.UserId == u.Id && m.SellerId == request.SellerId))
                .ExecuteUpdateAsync(setters => setters.SetProperty(u => u.IsDeleted, true), cancellationToken);

            await _context.SellerManagers
                .Where(m => m.SellerId == request.SellerId)
                .ExecuteUpdateAsync(setters => setters.SetProperty(m => m.IsDeleted, true), cancellationToken);

            seller.Seller.IsDeleted = true;
            await _context.SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
        foreach (var manager in seller.ManagersInfo)
        {
            try
            {
                await _emailHelper.SendAccountDeletedEmail(
                    manager.Email!,
                    $"{manager.FirstName} {manager.LastName}",
                    request.Reason
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send deletion email to manager: {Email} for SellerId: {SellerId}", manager.Email, request.SellerId);
            }
        }

        return Result.Success();
    }
}