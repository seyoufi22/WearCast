namespace WearCast.Api.Features.Factories.GetFactory;

public class GetFactoryHandler(
    ApplicationDbContext context
    ) : IRequestHandler<GetFactoryRequest, Result<GetFactoryResponse>>
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Result<GetFactoryResponse>> Handle(GetFactoryRequest request, CancellationToken cancellationToken)
    {
        
        var response = await _context.Factories
            .AsNoTracking()
            .Where(x => !x.IsDeleted) 
            .Select(x => new GetFactoryResponse(
                x.Id,
                x.Name,
                x.Email,
                x.PhoneNumber,
                x.CommercialRegisterNumber,
                x.TaxIdNumber,
                x.Description,
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
            return Result.Failure<GetFactoryResponse>(FactoryErrors.FactoryNotFound);
        }

        return Result.Success(response);
    }
}