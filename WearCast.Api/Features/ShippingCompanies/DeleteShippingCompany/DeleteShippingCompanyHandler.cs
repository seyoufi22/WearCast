namespace WearCast.Api.Features.ShippingCompanies.DeleteShippingCompany;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

public class DeleteShippingCompanyHandler(
    ApplicationDbContext context,
    EmailHelper emailHelper,
    ILogger<DeleteShippingCompanyHandler> logger
) : IRequestHandler<DeleteShippingCompanyRequest, Result>
{
    private readonly ApplicationDbContext _context = context;
    private readonly EmailHelper _emailHelper = emailHelper;
    private readonly ILogger<DeleteShippingCompanyHandler> _logger = logger;

    public async Task<Result> Handle(DeleteShippingCompanyRequest request, CancellationToken cancellationToken)
    {

        var companyData = await _context.ShippingCompanies
            .Where(x => x.Id == request.CompanyId && !x.IsDeleted)
            .Select(c => new
            {
                Company = c,
                ManagersInfo = c.Managers.Select(m => new
                {
                    Email = m.ApplicationUser!.Email,
                    FirstName = m.ApplicationUser.FirstName,
                    LastName = m.ApplicationUser.LastName
                }).ToList(),
                DriversInfo = c.Drivers.Select(d => new 
                {
                    Email = d.ApplicationUser!.Email,
                    FirstName = d.ApplicationUser.FirstName,
                    LastName = d.ApplicationUser.LastName
                }).ToList()
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (companyData is null)
            return Result.Failure(ShippingCompanyErrors.CompanyNotFound);

        using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            await _context.Users
                .Where(u => _context.ShippingCompanyManagers.Any(m => m.UserId == u.Id && m.ShippingCompanyId == request.CompanyId))
                .ExecuteUpdateAsync(setters => setters.SetProperty(u => u.IsDeleted, true), cancellationToken);

            await _context.ShippingCompanyManagers
                .Where(m => m.ShippingCompanyId == request.CompanyId)
                .ExecuteUpdateAsync(setters => setters.SetProperty(m => m.IsDeleted, true), cancellationToken);

            await _context.Users
                .Where(u => _context.Drivers.Any(d => d.UserId == u.Id && d.ShippingCompanyId == request.CompanyId))
                .ExecuteUpdateAsync(setters => setters.SetProperty(u => u.IsDeleted, true), cancellationToken);

            await _context.Drivers
                .Where(d => d.ShippingCompanyId == request.CompanyId)
                .ExecuteUpdateAsync(setters => setters.SetProperty(d => d.IsDeleted, true), cancellationToken);

            companyData.Company.IsDeleted = true;
            await _context.SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }

        foreach (var manager in companyData.ManagersInfo)
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
                _logger.LogError(ex, "Failed to send deletion email to manager: {Email} for ShippingCompanyId: {CompanyId}", manager.Email, request.CompanyId);
            }
        }

        foreach (var driver in companyData.DriversInfo)
        {
            try
            {
                await _emailHelper.SendAccountDeletedEmail(
                    driver.Email!,
                    $"{driver.FirstName} {driver.LastName}",
                    request.Reason 
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send deletion email to driver: {Email} for ShippingCompanyId: {CompanyId}", driver.Email, request.CompanyId);
            }
        }

        return Result.Success();
    }
}