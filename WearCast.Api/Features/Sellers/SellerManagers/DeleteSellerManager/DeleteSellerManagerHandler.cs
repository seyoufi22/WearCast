namespace WearCast.Api.Features.Sellers.SellerManagers.DeleteSellerManager;

public class DeleteSellerManagerHandler(
    ApplicationDbContext context,
    EmailHelper emailHelper,
    ILogger<DeleteSellerManagerHandler> logger
) : IRequestHandler<DeleteSellerManagerRequest, Result>
{
    private readonly ApplicationDbContext _context = context;
    private readonly EmailHelper _emailHelper = emailHelper;
    private readonly ILogger<DeleteSellerManagerHandler> _logger = logger;

    public async Task<Result> Handle(DeleteSellerManagerRequest request, CancellationToken cancellationToken)
    {

        var targetManagerData = await _context.SellerManagers
            .Where(m => m.Id == request.SellerManagerId && !m.IsDeleted) 
            .Select(m => new
            {
                m.Id,
                m.UserId,
                m.SellerId,
                Email = m.ApplicationUser!.Email,
                FirstName = m.ApplicationUser.FirstName,
                LastName = m.ApplicationUser.LastName
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (targetManagerData is null)
            return Result.Failure(SellerErrors.ManagerNotFound);

        if (targetManagerData.UserId == request.CurrentUserId)
        {
            return Result.Failure(SellerErrors.CannotDeleteYourself);
        }

        if (!request.IsSuperAdmin)
        {
            var currentManagerSellerId = await _context.SellerManagers
                .Where(m => m.UserId == request.CurrentUserId && !m.IsDeleted)
                .Select(m => m.SellerId)
                .FirstOrDefaultAsync(cancellationToken);

            if (currentManagerSellerId == 0)
                return Result.Failure(SellerErrors.CurrentManagerNotFound);

            if (currentManagerSellerId != targetManagerData.SellerId)
                return Result.Failure(SellerErrors.UnauthorizedToDeleteManager);
        }

        using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            await _context.SellerManagers
                .Where(m => m.Id == request.SellerManagerId)
                .ExecuteUpdateAsync(setters => setters.SetProperty(m => m.IsDeleted, true), cancellationToken);

            await _context.Users
                .Where(u => u.Id == targetManagerData.UserId)
                .ExecuteUpdateAsync(setters => setters.SetProperty(u => u.IsDeleted, true), cancellationToken);

            await transaction.CommitAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            _logger.LogError(ex, "Transaction failed while deleting SellerManagerId: {SellerManagerId} by CurrentUserId: {CurrentUserId}", request.SellerManagerId, request.CurrentUserId);
            throw;
        }

        try
        {
            await _emailHelper.SendAccountDeletedEmail(
                targetManagerData.Email!,
                $"{targetManagerData.FirstName} {targetManagerData.LastName}",
                request.Reason
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send deletion email to manager: {Email} for SellerManagerId: {SellerManagerId}", targetManagerData.Email, request.SellerManagerId);
        }

        return Result.Success();
    }
}