namespace WearCast.Api.Features.DesignedProductManagement.CustomerDesigns.DeleteCustomerDesign
{
    public class DeleteCustomerDesignHandler(
        ApplicationDbContext context,
        IHttpContextAccessor httpContextAccessor) : IRequestHandler<DeleteCustomerDesignRequest, Result>
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<Result> Handle(DeleteCustomerDesignRequest request, CancellationToken cancellationToken)
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
                return Result.Failure(CustomerDesignErrors.DesignNotFound);
            }

            customerDesign.IsDeleted = true;

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
