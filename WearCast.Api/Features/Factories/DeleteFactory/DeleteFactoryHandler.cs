namespace WearCast.Api.Features.Factories.DeleteFactory;

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

        using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            await _context.Users
                .Where(u => _context.FactoryManagers.Any(m => m.UserId == u.Id && m.FactoryId == request.FactoryId))
                .ExecuteUpdateAsync(setters => setters.SetProperty(u => u.IsDeleted, true), cancellationToken);

            await _context.FactoryManagers
                .Where(m => m.FactoryId == request.FactoryId)
                .ExecuteUpdateAsync(setters => setters.SetProperty(m => m.IsDeleted, true), cancellationToken);

            factoryData.Factory.IsDeleted = true;
            await _context.SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }

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