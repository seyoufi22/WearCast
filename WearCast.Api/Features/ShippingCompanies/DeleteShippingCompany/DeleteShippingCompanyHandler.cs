namespace WearCast.Api.Features.ShippingCompanies.DeleteShippingCompany;

public class DeleteShippingCompanyHandler(
    ApplicationDbContext context,
    EmailHelper emailHelper
) : IRequestHandler<DeleteShippingCompanyRequest, Result>
{
    private readonly ApplicationDbContext _context = context;
    private readonly EmailHelper _emailHelper = emailHelper;

    public async Task<Result> Handle(DeleteShippingCompanyRequest request, CancellationToken cancellationToken)
    {
        var company = await _context.ShippingCompanies
            .Include(c => c.Managers)
                .ThenInclude(m => m.ApplicationUser)
            .FirstOrDefaultAsync(x => x.Id == request.CompanyId && !x.IsDeleted, cancellationToken);

        if (company is null)
            return Result.Failure(ShippingCompanyErrors.CompanyNotFound);

        company.IsDeleted = true;

        foreach (var manager in company.Managers)
        {
            manager.IsDeleted = true;
            manager.ApplicationUser!.IsDeleted = true;
        }

        await _context.SaveChangesAsync(cancellationToken);

        foreach (var manager in company.Managers)
            await _emailHelper.SendAccountDeletedEmail(
                manager.ApplicationUser!.Email!,
                $"{manager.ApplicationUser.FirstName} {manager.ApplicationUser.LastName}",
                request.Reason
            );

        return Result.Success();
    }
}