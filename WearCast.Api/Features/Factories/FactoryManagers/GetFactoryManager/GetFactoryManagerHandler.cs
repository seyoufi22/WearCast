namespace WearCast.Api.Features.Factories.FactoryManagers.GetFactoryManager;

public class GetFactoryManagerHandler(
    ApplicationDbContext context,
    IHttpContextAccessor httpContextAccessor
    ) : IRequestHandler<GetFactoryManagerRequest, Result<GetFactoryManagerResponse>>
{
    private readonly ApplicationDbContext _context = context;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public async Task<Result<GetFactoryManagerResponse>> Handle(GetFactoryManagerRequest request, CancellationToken cancellationToken)
    {
        var user = _httpContextAccessor.HttpContext!.User;
        int targetManagerId;

        if (user.IsSuperAdmin())
        {
            if (!request.ProvidedManagerId.HasValue)
            {
                return Result.Failure<GetFactoryManagerResponse>(new Error("Validation.MissingId", "SuperAdmin must provide a target ManagerId to get.", StatusCodes.Status400BadRequest));
            }

            targetManagerId = request.ProvidedManagerId.Value;
        }
        else
        {
            targetManagerId = user.GetFactoryManagerId()!.Value;
        }

        var response = await _context.Users
            .AsNoTracking()
            .Where(u => u.FactoryManager != null && u.FactoryManager.Id == targetManagerId)
            .Select(u => new GetFactoryManagerResponse(
                u.FactoryManager!.Id,
                u.FirstName,
                u.LastName,
                u.PhoneNumber,
                u.Email
            ))
            .FirstOrDefaultAsync(cancellationToken);

        if (response == null)
        {
            return Result.Failure<GetFactoryManagerResponse>(FactoryManagerErrors.NotFound);
        }

        return Result.Success(response);
    }
}