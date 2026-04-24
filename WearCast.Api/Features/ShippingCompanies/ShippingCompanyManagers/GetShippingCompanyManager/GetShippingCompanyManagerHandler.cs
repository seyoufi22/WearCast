namespace WearCast.Api.Features.ShippingCompanies.ShippingCompanyManagers.GetShippingCompanyManager;

public class GetShippingCompanyManagerHandler(
    ApplicationDbContext context,
    IHttpContextAccessor httpContextAccessor
    ) : IRequestHandler<GetShippingCompanyManagerRequest, Result<GetShippingCompanyManagerResponse>>
{
    private readonly ApplicationDbContext _context = context;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public async Task<Result<GetShippingCompanyManagerResponse>> Handle(GetShippingCompanyManagerRequest request, CancellationToken cancellationToken)
    {
        var user = _httpContextAccessor.HttpContext!.User;
        int targetManagerId;

        if (user.IsSuperAdmin())
        {
            if (!request.ProvidedManagerId.HasValue)
            {
                return Result.Failure<GetShippingCompanyManagerResponse>(new Error("Validation.MissingId", "SuperAdmin must provide a target ManagerId to get.", StatusCodes.Status400BadRequest));
            }

            targetManagerId = request.ProvidedManagerId.Value;
        }
        else
        {
            targetManagerId = user.GetShippingCompanyManagerId()!.Value;
        }

        var response = await _context.Users
            .AsNoTracking()
            .Where(u => u.ShippingCompanyManager != null && u.ShippingCompanyManager.Id == targetManagerId)
            .Select(u => new GetShippingCompanyManagerResponse(
                u.ShippingCompanyManager!.Id,
                u.FirstName,
                u.LastName,
                u.PhoneNumber
            ))
            .FirstOrDefaultAsync(cancellationToken);

        if (response == null)
        {
            return Result.Failure<GetShippingCompanyManagerResponse>(ShippingCompanyManagerErrors.NotFound);
        }

        return Result.Success(response);
    }
}