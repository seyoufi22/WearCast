using WearCast.Api.Common.Helper;
using WearCast.Api.Common.Views;

namespace WearCast.Api.Features.Customers.GetAllCustomers;

public class GetAllCustomersHandler(ApplicationDbContext context)
    : IRequestHandler<GetAllCustomersRequest, Result<PagingViewModel<GetAllCustomersResponse>>>
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Result<PagingViewModel<GetAllCustomersResponse>>> Handle(GetAllCustomersRequest request, CancellationToken cancellationToken)
    {
        var query = _context.Customers
            .Include(c => c.ApplicationUser)
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var term = request.SearchTerm.Trim();
            query = query.Where(c =>
                c.ApplicationUser!.FirstName.Contains(term) ||
                c.ApplicationUser.LastName.Contains(term));
        }

        var projectedQuery = query.Select(c => new GetAllCustomersResponse(
            c.Id,
            c.ApplicationUser!.FirstName,
            c.ApplicationUser.LastName,
            c.ApplicationUser.Email!,
            c.ApplicationUser.PhoneNumber ?? string.Empty,
            c.ProfileImageUrl,
            c.Address != null ? c.Address.City : null
        ));

        var pagedResult = await PagingHelper.CreateAsync(projectedQuery, request.PageIndex, request.PageSize);

        return Result.Success(pagedResult);
    }
}