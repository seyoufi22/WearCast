namespace WearCast.Api.Features.ShippingCompanies.GetShippingCompany;

public class GetShippingCompanyHandler(
    ApplicationDbContext context
    ) : IRequestHandler<GetShippingCompanyRequest, Result<GetShippingCompanyResponse>>
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Result<GetShippingCompanyResponse>> Handle(GetShippingCompanyRequest request, CancellationToken cancellationToken)
    {
        var response = await _context.ShippingCompanies
            .AsNoTracking()
            .Where(x => !x.IsDeleted)
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