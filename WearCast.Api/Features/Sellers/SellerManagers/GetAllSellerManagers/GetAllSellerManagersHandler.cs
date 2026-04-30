using WearCast.Api.Features.Sellers.SellerManagers.GetSellerManager;

namespace WearCast.Api.Features.Sellers.SellerManagers.GetAllSellerManagers;

public class GetAllSellerManagersHandler(
    ApplicationDbContext context,
    IHttpContextAccessor httpContextAccessor
) : IRequestHandler<GetAllSellerManagersRequest, Result<List<GetSellerManagerResponse>>>
{
    private readonly ApplicationDbContext _context = context;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public async Task<Result<List<GetSellerManagerResponse>>> Handle(GetAllSellerManagersRequest request, CancellationToken cancellationToken)
    {
        var user = _httpContextAccessor.HttpContext!.User;
        int targetSellerId;

        if (user.IsSuperAdmin())
        {
            if (!request.ProvidedSellerId.HasValue)
                return Result.Failure<List<GetSellerManagerResponse>>(new Error(
                    "Validation.MissingId",
                    "SuperAdmin must provide a target SellerId to get.",
                    StatusCodes.Status400BadRequest));

            targetSellerId = request.ProvidedSellerId.Value;
        }
        else
        {
            targetSellerId = user.GetSellerId()!.Value;
        }

        var managers = await _context.Users
            .AsNoTracking()
            .Where(u => u.SellerManager != null
                     && u.SellerManager.SellerId == targetSellerId
                     && !u.SellerManager.IsDeleted)
            .Select(u => new GetSellerManagerResponse(
                u.SellerManager!.Id,
                u.FirstName,
                u.LastName,
                u.PhoneNumber
            ))
            .ToListAsync(cancellationToken);

        return Result.Success(managers);
    }
}