namespace WearCast.Api.Features.DesignedProductManagement.CustomerDesigns.UpdateCustomerDesign
{
    public class UpdateCustomerDesignHandler(
        ApplicationDbContext context,
        IHttpContextAccessor httpContextAccessor
        ) : IRequestHandler<UpdateCustomerDesignRequest, Result<CustomerDesignResponse>>
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<Result<CustomerDesignResponse>> Handle(UpdateCustomerDesignRequest request, CancellationToken cancellationToken)
        {
            var user = _httpContextAccessor.HttpContext!.User;

            var customerId = user.GetCustomerId();
            if (customerId == null)
            {
                return Result.Failure<CustomerDesignResponse>(AuthErrors.Forbidden);
            }

            var customerDesign = await _context.CustomerDesigns
                .FirstOrDefaultAsync(d =>
                    d.Id == request.Id &&
                    d.CustomerId == customerId.Value,
                cancellationToken);

            if (customerDesign == null)
            {
                return Result.Failure<CustomerDesignResponse>(CustomerDesignErrors.DesignNotFound);
            }

            customerDesign.ViewDesignsJson = request.ViewDesignsJson;

            // لو غير اللون، نحدثه كمان
            if (customerDesign.DesignedProductColorId != request.NewProductColorId)
            {
                // ممكن هنا تعمل تشيك سريع لو حابب تتأكد إن اللون ده لسه موجود/متاح
                customerDesign.DesignedProductColorId = request.NewProductColorId;
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success(new CustomerDesignResponse(customerDesign.Id));
        }
    }
}
