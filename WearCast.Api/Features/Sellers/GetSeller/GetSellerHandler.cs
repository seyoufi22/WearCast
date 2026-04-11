namespace WearCast.Api.Features.Sellers.GetSeller;

public class GetSellerHandler(
    ApplicationDbContext context,
    IHttpContextAccessor httpContextAccessor
    ) : IRequestHandler<GetSellerRequest, Result<GetSellerResponse>>
{
    private readonly ApplicationDbContext _context = context;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public async Task<Result<GetSellerResponse>> Handle(GetSellerRequest request, CancellationToken cancellationToken)
    {
        var user = _httpContextAccessor.HttpContext!.User;
        int targetSellerId;

        if (user.IsSuperAdmin())
        {
            if (!request.ProvidedSellerId.HasValue)
            {
                return Result.Failure<GetSellerResponse>(new Error("Validation.MissingId", "SuperAdmin must provide a target SellerId to get.", StatusCodes.Status400BadRequest));
            }

            targetSellerId = request.ProvidedSellerId.Value;
        }
        else
        {
            targetSellerId = user.GetSellerId()!.Value;
        }

        var response = await _context.Sellers
            .AsNoTracking()
            .Where(s => s.Id == targetSellerId) 
            .Select(s => new GetSellerResponse(
                s.Id,
                s.Name,
                s.Email,
                s.PhoneNumber,
                s.CommercialRegisterNumber,
                s.TaxIdNumber,
                s.Description,
                s.LogoUrl,
                s.Address != null ? new AddressDto(
                    s.Address.State,
                    s.Address.City,
                    s.Address.Street,
                    s.Address.BuildingNumber
                ) : null
            ))
            .FirstOrDefaultAsync(cancellationToken);

        if (response == null)
        {
            return Result.Failure<GetSellerResponse>(SellerErrors.SellerNotFound);
        }

        return Result.Success(response);
    }
}