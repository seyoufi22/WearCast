namespace WearCast.Api.Features.Sellers.SellerManagers.GetSellerManager;

public class GetSellerManagerHandler(
    ApplicationDbContext context,
    IHttpContextAccessor httpContextAccessor
    ) : IRequestHandler<GetSellerManagerRequest, Result<GetSellerManagerResponse>>
{
    private readonly ApplicationDbContext _context = context;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public async Task<Result<GetSellerManagerResponse>> Handle(GetSellerManagerRequest request, CancellationToken cancellationToken)
    {
        var user = _httpContextAccessor.HttpContext!.User;
        int targetManagerId;

        if (user.IsSuperAdmin())
        {
            if (!request.ProvidedManagerId.HasValue)
            {
                return Result.Failure<GetSellerManagerResponse>(new Error("Validation.MissingId", "SuperAdmin must provide a target ManagerId to get.", StatusCodes.Status400BadRequest));
            }

            targetManagerId = request.ProvidedManagerId.Value;
        }
        else
        {
            targetManagerId = user.GetSellerManagerId()!.Value;
        }

        var response = await _context.Users
            .AsNoTracking()
            .Where(u => u.SellerManager != null && u.SellerManager.Id == targetManagerId)
            .Select(u => new GetSellerManagerResponse(
                u.SellerManager!.Id,
                u.FirstName,
                u.LastName,
                u.PhoneNumber,
                u.Email
            ))
            .FirstOrDefaultAsync(cancellationToken);

        if (response == null)
        {
            return Result.Failure<GetSellerManagerResponse>(SellerManagerErrors.NotFound);
        }

        return Result.Success(response);
    }
}