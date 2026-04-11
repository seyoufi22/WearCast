namespace WearCast.Api.Features.Factories.GetFactory;

public class GetFactoryHandler(
    ApplicationDbContext context,
    IHttpContextAccessor httpContextAccessor
    ) : IRequestHandler<GetFactoryRequest, Result<GetFactoryResponse>>
{
    private readonly ApplicationDbContext _context = context;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public async Task<Result<GetFactoryResponse>> Handle(GetFactoryRequest request, CancellationToken cancellationToken)
    {
        var user = _httpContextAccessor.HttpContext!.User;
        int targetFactoryId;

        if (user.IsSuperAdmin())
        {
            if (!request.ProvidedFactoryId.HasValue)
            {
                return Result.Failure<GetFactoryResponse>(new Error("Validation.MissingId", "SuperAdmin must provide a target FactoryId to get.", StatusCodes.Status400BadRequest));
            }

            targetFactoryId = request.ProvidedFactoryId.Value;
        }
        else
        {
            targetFactoryId = user.GetFactoryId()!.Value;
        }

        var response = await _context.Factories
            .AsNoTracking()
            .Where(x => x.Id == targetFactoryId && !x.IsDeleted) 
            .Select(x => new GetFactoryResponse(
                x.Id,
                x.Name,
                x.Email,
                x.PhoneNumber,
                x.CommercialRegisterNumber,
                x.TaxIdNumber,
                x.Description,
                x.Address != null ? new AddressDto(
                    x.Address.State,
                    x.Address.City,
                    x.Address.Street,
                    x.Address.BuildingNumber
                ) : null
            ))
            .FirstOrDefaultAsync(cancellationToken);

        if (response == null)
        {
            return Result.Failure<GetFactoryResponse>(FactoryErrors.FactoryNotFound);
        }

        return Result.Success(response);
    }
}