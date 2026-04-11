namespace WearCast.Api.Features.ShippingCompanies.GetShippingCompany;

public class GetShippingCompanyHandler(
    ApplicationDbContext context,
    IHttpContextAccessor httpContextAccessor
    ) : IRequestHandler<GetShippingCompanyRequest, Result<GetShippingCompanyResponse>>
{
    private readonly ApplicationDbContext _context = context;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public async Task<Result<GetShippingCompanyResponse>> Handle(GetShippingCompanyRequest request, CancellationToken cancellationToken)
    {
        var user = _httpContextAccessor.HttpContext!.User;
        int targetCompanyId;

        if (user.IsSuperAdmin())
        {
            if (!request.ProvidedCompanyId.HasValue)
            {
                return Result.Failure<GetShippingCompanyResponse>(new Error("Validation.MissingId", "SuperAdmin must provide a target CompanyId to get.", StatusCodes.Status400BadRequest));
            }

            targetCompanyId = request.ProvidedCompanyId.Value;
        }
        else
        {
            targetCompanyId = user.GetShippingCompanyId()!.Value;
        }

        var response = await _context.ShippingCompanies
            .AsNoTracking()
            .Where(x => x.Id == targetCompanyId && !x.IsDeleted)
            .Select(x => new GetShippingCompanyResponse(
                x.Id,
                x.Name,
                x.Email,
                x.PhoneNumber,
                x.CommercialRegisterNumber,
                x.TaxIdNumber,
                x.Description,
                x.DeliveryFee,
                x.LogoUrl,
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
            return Result.Failure<GetShippingCompanyResponse>(ShippingCompanyErrors.CompanyNotFound);
        }

        return Result.Success(response);
    }
}