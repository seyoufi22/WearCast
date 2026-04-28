using WearCast.Api.Common.Helper;
using WearCast.Api.Common.Views;

namespace WearCast.Api.Features.Sellers.GetAllSellers;

public class GetAllSellersHandler(ApplicationDbContext context)
    : IRequestHandler<GetAllSellersRequest, Result<PagingViewModel<GetAllSellersResponse>>>
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Result<PagingViewModel<GetAllSellersResponse>>> Handle(GetAllSellersRequest request, CancellationToken cancellationToken)
    {
        var query = _context.Sellers.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            query = query.Where(s => s.Name.Contains(request.SearchTerm.Trim()));
        }

        var projectedQuery = query.Select(s => new GetAllSellersResponse(
            s.Id,
            s.Name,
            s.Email,
            s.PhoneNumber,
            s.LogoUrl,
            s.Address != null ? s.Address.City : null
        ));

        var pagedResult = await PagingHelper.CreateAsync(projectedQuery, request.PageIndex, request.PageSize);

        return Result.Success(pagedResult);
    }
}