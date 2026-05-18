namespace WearCast.Api.Features.Factories.FactoryManagers.DeleteFactoryManager;

public class DeleteFactoryManagerHandler(
    ApplicationDbContext context,
    EmailHelper emailHelper,
    ILogger<DeleteFactoryManagerHandler> logger
) : IRequestHandler<DeleteFactoryManagerRequest, Result>
{
    private readonly ApplicationDbContext _context = context;
    private readonly EmailHelper _emailHelper = emailHelper;
    private readonly ILogger<DeleteFactoryManagerHandler> _logger = logger;

    public async Task<Result> Handle(DeleteFactoryManagerRequest request, CancellationToken cancellationToken)
    {
        var targetManagerData = await _context.FactoryManagers
            .Where(m => m.Id == request.FactoryManagerId && !m.IsDeleted)
            .Select(m => new
            {
                m.Id,
                m.UserId,
                m.FactoryId,
                Email = m.ApplicationUser!.Email,
                FirstName = m.ApplicationUser.FirstName,
                LastName = m.ApplicationUser.LastName
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (targetManagerData is null)
            return Result.Failure(FactoryManagerErrors.NotFound);

        if (targetManagerData.UserId == request.CurrentUserId)
        {
            return Result.Failure(FactoryManagerErrors.CannotDeleteYourself);
        }

        var activeManagersCount = await _context.FactoryManagers
            .CountAsync(m => m.FactoryId == targetManagerData.FactoryId && !m.IsDeleted, cancellationToken);

        if (activeManagersCount <= 1)
        {
            return Result.Failure(FactoryManagerErrors.CannotDeleteLastManager);
        }

        if (!request.IsAdmin)
        {
            var currentManagerFactoryId = await _context.FactoryManagers
                .Where(m => m.UserId == request.CurrentUserId && !m.IsDeleted)
                .Select(m => m.FactoryId)
                .FirstOrDefaultAsync(cancellationToken);

            if (currentManagerFactoryId == 0)
                return Result.Failure(FactoryManagerErrors.CurrentManagerNotFound);

            if (currentManagerFactoryId != targetManagerData.FactoryId)
                return Result.Failure(FactoryManagerErrors.UnauthorizedToDeleteManager);
        }

        using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            await _context.FactoryManagers
                .Where(m => m.Id == request.FactoryManagerId)
                .ExecuteUpdateAsync(setters => setters.SetProperty(m => m.IsDeleted, true), cancellationToken);

            await _context.Users
                .Where(u => u.Id == targetManagerData.UserId)
                .ExecuteUpdateAsync(setters => setters.SetProperty(u => u.IsDeleted, true), cancellationToken);

            await transaction.CommitAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            _logger.LogError(ex, "Transaction failed while deleting FactoryManagerId: {FactoryManagerId} by CurrentUserId: {CurrentUserId}", request.FactoryManagerId, request.CurrentUserId);
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
            _logger.LogError(ex, "Failed to send deletion email to manager: {Email} for FactoryManagerId: {FactoryManagerId}", targetManagerData.Email, request.FactoryManagerId);
        }

        return Result.Success();
    }
}