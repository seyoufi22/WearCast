namespace WearCast.Api.Features.Factories.DeleteFactory;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

public class DeleteFactoryHandler(
    ApplicationDbContext context,
    EmailHelper emailHelper,
    ILogger<DeleteFactoryHandler> logger
) : IRequestHandler<DeleteFactoryRequest, Result>
{
    private readonly ApplicationDbContext _context = context;
    private readonly EmailHelper _emailHelper = emailHelper;
    private readonly ILogger<DeleteFactoryHandler> _logger = logger;

    public async Task<Result> Handle(DeleteFactoryRequest request, CancellationToken cancellationToken)
    {
        // 1. Fetch factory and managers' info
        var factoryData = await _context.Factories
            .Where(x => x.Id == request.FactoryId && !x.IsDeleted)
            .Select(f => new
            {
                Factory = f,
                ManagersInfo = f.Managers.Select(m => new
                {
                    Email = m.ApplicationUser!.Email,
                    FirstName = m.ApplicationUser.FirstName,
                    LastName = m.ApplicationUser.LastName
                }).ToList()
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (factoryData is null)
            return Result.Failure(FactoryErrors.FactoryNotFound);

        // 2. Begin Transaction
        using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            // Soft delete users related to the factory managers
            await _context.Users
                .Where(u => _context.FactoryManagers.Any(m => m.UserId == u.Id && m.FactoryId == request.FactoryId))
                .ExecuteUpdateAsync(setters => setters.SetProperty(u => u.IsDeleted, true), cancellationToken);

            // Soft delete factory managers
            await _context.FactoryManagers
                .Where(m => m.FactoryId == request.FactoryId)
                .ExecuteUpdateAsync(setters => setters.SetProperty(m => m.IsDeleted, true), cancellationToken);

            // ---> NEW ADDITIONS START HERE <---

            // Soft delete all DesignedProductColors linked to the DesignedProducts of this Factory
            // Note: Adjust 'DesignedProductColors' and 'DesignedProducts' to match your actual DbSet property names
            await _context.DesignedProductColors
                .Where(color => _context.DesignedProducts.Any(dp => dp.Id == color.DesignedProductId && dp.FactoryId == request.FactoryId))
                .ExecuteUpdateAsync(setters => setters.SetProperty(color => color.IsDeleted, true), cancellationToken);

            // Soft delete all DesignedProducts linked to the Factory
            await _context.DesignedProducts
                .Where(dp => dp.FactoryId == request.FactoryId)
                .ExecuteUpdateAsync(setters => setters.SetProperty(dp => dp.IsDeleted, true), cancellationToken);

            // ---> NEW ADDITIONS END HERE <---

            // Soft delete the factory itself
            factoryData.Factory.IsDeleted = true;
            await _context.SaveChangesAsync(cancellationToken);

            // Commit all changes atomically
            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }

        // 3. Send emails to managers (happens outside the transaction scope to prevent email sending if DB fails)
        foreach (var manager in factoryData.ManagersInfo)
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
                _logger.LogError(ex, "Failed to send deletion email to manager: {Email} for FactoryId: {FactoryId}", manager.Email, request.FactoryId);
            }
        }

        return Result.Success();
    }
}