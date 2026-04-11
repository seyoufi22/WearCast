namespace WearCast.Api.Features.Sellers.GetSeller;

public class GetSellerHandler(ApplicationDbContext context) : IRequestHandler<GetSellerRequest, Result<GetSellerResponse>>
{
    private readonly ApplicationDbContext _context = context;
    public async Task<Result<GetSellerResponse>> Handle(GetSellerRequest request, CancellationToken cancellationToken)
    {
        var seller = await _context.Sellers
            .Where(s => s.Id == request.Id)
            .Select(s => new GetSellerResponse(
                s.Name,
                s.Email,
                s.PhoneNumber,
                s.CommercialRegisterNumber,

                s.TaxIdNumber,
                s.Description,
                s.LogoUrl,
                s.Address == null ? null : new AddressDto(
                    s.Address.State,
                    s.Address.City,
                    s.Address.Street,
                    s.Address.BuildingNumber
                )
            ))
            .FirstOrDefaultAsync(cancellationToken);
        if (seller == null)
        {
            return Result.Failure<GetSellerResponse>(SellerErrors.SellerNotFound);
        }
        return Result.Success(seller);
    }
}
