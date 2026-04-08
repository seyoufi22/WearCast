namespace WearCast.Api.Features.DesignedProductManagement.CustomerDesigns.AddCustomerDesign
{
    public class AddCustomerDesignHandler(
        ApplicationDbContext context,
        IHttpContextAccessor httpContextAccessor
        ) : IRequestHandler<AddCustomerDesignRequest, Result<CustomerDesignResponse>>
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<Result<CustomerDesignResponse>> Handle(AddCustomerDesignRequest request, CancellationToken cancellationToken)
        {
            var user = _httpContextAccessor.HttpContext!.User;

            var customerId = user.GetCustomerId();
            if (customerId == null)
            {
                return Result.Failure<CustomerDesignResponse>(AuthErrors.Forbidden);
            }

            var isValidProductAndColor = await _context.DesignedProductColors
                .AnyAsync(c =>
                    c.Id == request.ProductColorId &&
                    c.DesignedProductId == request.ProductId,
                cancellationToken);

            if (!isValidProductAndColor)
            {
                return Result.Failure<CustomerDesignResponse>(new Error("Design.InvalidColor", "The selected color does not exist for this product.", 400));
            }

            var customerDesign = new CustomerDesign
            {
                ViewDesignsJson = request.ViewDesignsJson,
                CustomerId = customerId.Value,
                DesignedProductId = request.ProductId,
                DesignedProductColorId = request.ProductColorId
            };

            _context.CustomerDesigns.Add(customerDesign);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success(new CustomerDesignResponse(customerDesign.Id));
        }
    }
}
