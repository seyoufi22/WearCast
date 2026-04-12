namespace WearCast.Api.Features.Customers.GetCustomer;

public class GetCustomerHandler(
    ApplicationDbContext context,
    IHttpContextAccessor httpContextAccessor,
    IMapper mapper
    ) : IRequestHandler<GetCustomerRequest, Result<GetCustomerResponse>>
{
    private readonly ApplicationDbContext _context = context;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<GetCustomerResponse>> Handle(GetCustomerRequest request, CancellationToken cancellationToken)
    {
        var user = _httpContextAccessor.HttpContext!.User;

        int targetCustomerId;

        if (user.IsSuperAdmin())
        {
            if (!request.ProvidedCustomerId.HasValue)
            {
                return Result.Failure<GetCustomerResponse>(new Error("Validation.MissingId", "SuperAdmin must provide a target CustomerId to get.", StatusCodes.Status400BadRequest));
            }

            targetCustomerId = request.ProvidedCustomerId.Value;
        }
        else
        {
            targetCustomerId = user.GetCustomerId()!.Value;
        }

        var response = await _context.Customers
                .AsNoTracking()
                .Where(x => x.Id == targetCustomerId)
                .Select(x => new GetCustomerResponse(
                    x.Id,
                    x.ApplicationUser!.FirstName,
                    x.ApplicationUser.LastName,
                    x.ApplicationUser.PhoneNumber ?? string.Empty,
                    x.Address != null ? new AddressDto(
                        x.Address.State,        
                        x.Address.City,           
                        x.Address.Street,        
                        x.Address.BuildingNumber  
                    ) : null! 
                ))
                .FirstOrDefaultAsync(cancellationToken);

        if (response == null)
        {
            return Result.Failure<GetCustomerResponse>(CustomerErrors.CustomerNotFound);
        }
        return Result.Success(response);
    }
}