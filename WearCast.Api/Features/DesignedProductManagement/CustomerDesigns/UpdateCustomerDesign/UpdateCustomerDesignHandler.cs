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

            if (customerDesign.DesignedProductColorId != request.NewProductColorId)
            {
                // ممكن هنا تعمل تشيك سريع لو حابب تتأكد إن اللون ده لسه موجود/متاح

                var isValidColor = await _context.DesignedProductColors
                    .AnyAsync(c =>
                        c.Id == request.NewProductColorId &&
                        c.DesignedProductId == customerDesign.DesignedProductId, // لازم اللون يكون تبع نفس المنتج
                    cancellationToken);

                if (!isValidColor)
                {
                    return Result.Failure<CustomerDesignResponse>(new Error("Design.InvalidColor", "The selected color does not exist for this product.", 400));
                }
                customerDesign.DesignedProductColorId = request.NewProductColorId;
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success(new CustomerDesignResponse(customerDesign.Id));
        }
    }
}
