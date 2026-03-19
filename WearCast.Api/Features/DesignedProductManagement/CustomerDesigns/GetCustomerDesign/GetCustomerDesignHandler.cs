namespace WearCast.Api.Features.DesignedProductManagement.CustomerDesigns.GetCustomerDesign
{
    public class GetCustomerDesignHandler(
        ApplicationDbContext context,
        IHttpContextAccessor httpContextAccessor
        ) : IRequestHandler<GetCustomerDesignRequest, Result<GetCustomerDesignResponse>>
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<Result<GetCustomerDesignResponse>> Handle(GetCustomerDesignRequest request, CancellationToken cancellationToken)
        {
            var user = _httpContextAccessor.HttpContext!.User;

            var customerId = user.GetCustomerId();

            var customerDesign = await _context.CustomerDesigns
                .AsNoTracking()
                .FirstOrDefaultAsync(d =>
                    d.Id == request.Id &&
                    d.CustomerId == customerId.Value,
                cancellationToken);

            if (customerDesign == null)
            {
                return Result.Failure<GetCustomerDesignResponse>(CustomerDesignErrors.DesignNotFound);
            }

            var response = new GetCustomerDesignResponse(
                customerDesign.Id,
                customerDesign.DesignedProductId,
                customerDesign.DesignedProductColorId,
                customerDesign.ViewDesignsJson
            );

            return Result.Success(response);
        }
    }
}
