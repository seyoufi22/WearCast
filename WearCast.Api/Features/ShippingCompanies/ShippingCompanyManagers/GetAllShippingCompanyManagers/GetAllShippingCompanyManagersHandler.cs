using WearCast.Api.Features.ShippingCompanies.ShippingCompanyManagers.GetShippingCompanyManager;

namespace WearCast.Api.Features.ShippingCompanies.ShippingCompanyManagers.GetAllShippingCompanyManagers;

public class GetAllShippingCompanyManagersHandler(
    ApplicationDbContext context
) : IRequestHandler<GetAllShippingCompanyManagersRequest, Result<List<GetShippingCompanyManagerResponse>>>
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Result<List<GetShippingCompanyManagerResponse>>> Handle(GetAllShippingCompanyManagersRequest request, CancellationToken cancellationToken)
    {
        var managers = await _context.Users
            .AsNoTracking()
            .Where(u => u.ShippingCompanyManager != null
                     && !u.ShippingCompanyManager.IsDeleted)
            .Select(u => new GetShippingCompanyManagerResponse(
                u.ShippingCompanyManager!.Id,
                u.FirstName,
                u.LastName,
                u.PhoneNumber,
                u.Email
            ))
            .ToListAsync(cancellationToken);

        return Result.Success(managers);
    }
}