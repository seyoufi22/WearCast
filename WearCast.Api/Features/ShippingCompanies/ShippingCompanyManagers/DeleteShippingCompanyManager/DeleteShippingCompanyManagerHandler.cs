
namespace WearCast.Api.Features.ShippingCompanies.ShippingCompanyManagers.DeleteShippingCompanyManager;

public class DeleteShippingCompanyManagerHandler(
    ApplicationDbContext context,
    EmailHelper emailHelper,
    ILogger<DeleteShippingCompanyManagerHandler> logger
) : IRequestHandler<DeleteShippingCompanyManagerRequest, Result>
{
    private readonly ApplicationDbContext _context = context;
    private readonly EmailHelper _emailHelper = emailHelper;
    private readonly ILogger<DeleteShippingCompanyManagerHandler> _logger = logger;

    public async Task<Result> Handle(DeleteShippingCompanyManagerRequest request, CancellationToken cancellationToken)
    {
        var targetManagerData = await _context.ShippingCompanyManagers
            .Where(m => m.Id == request.ShippingCompanyManagerId && !m.IsDeleted)
            .Select(m => new
            {
                m.Id,
                m.UserId,
                m.ShippingCompanyId, 
                Email = m.ApplicationUser!.Email,
                FirstName = m.ApplicationUser.FirstName,
                LastName = m.ApplicationUser.LastName
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (targetManagerData is null)
            return Result.Failure(ShippingCompanyManagerErrors.NotFound);

        if (targetManagerData.UserId == request.CurrentUserId)
        {
            return Result.Failure(ShippingCompanyManagerErrors.CannotDeleteYourself);
        }

        var activeManagersCount = await _context.ShippingCompanyManagers
            .CountAsync(m => m.ShippingCompanyId == targetManagerData.ShippingCompanyId && !m.IsDeleted, cancellationToken);

        if (activeManagersCount <= 1)
        {
            return Result.Failure(ShippingCompanyManagerErrors.CannotDeleteLastManager);
        }

        if (!request.IsAdmin)
        {
            var currentManagerCompanyId = await _context.ShippingCompanyManagers
                .Where(m => m.UserId == request.CurrentUserId && !m.IsDeleted)
                .Select(m => m.ShippingCompanyId)
                .FirstOrDefaultAsync(cancellationToken);

            if (currentManagerCompanyId == 0)
                return Result.Failure(ShippingCompanyManagerErrors.CurrentManagerNotFound);

            if (currentManagerCompanyId != targetManagerData.ShippingCompanyId)
                return Result.Failure(ShippingCompanyManagerErrors.UnauthorizedToDeleteManager);
        }

        using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            await _context.ShippingCompanyManagers
                .Where(m => m.Id == request.ShippingCompanyManagerId)
                .ExecuteUpdateAsync(setters => setters.SetProperty(m => m.IsDeleted, true), cancellationToken);

            await _context.Users
                .Where(u => u.Id == targetManagerData.UserId)
                .ExecuteUpdateAsync(setters => setters.SetProperty(u => u.IsDeleted, true), cancellationToken);

            await transaction.CommitAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            _logger.LogError(ex, "Transaction failed while deleting ShippingCompanyManagerId: {ShippingCompanyManagerId} by CurrentUserId: {CurrentUserId}", request.ShippingCompanyManagerId, request.CurrentUserId);
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
            _logger.LogError(ex, "Failed to send deletion email to manager: {Email} for ShippingCompanyManagerId: {ShippingCompanyManagerId}", targetManagerData.Email, request.ShippingCompanyManagerId);
        }

        return Result.Success();
    }
}