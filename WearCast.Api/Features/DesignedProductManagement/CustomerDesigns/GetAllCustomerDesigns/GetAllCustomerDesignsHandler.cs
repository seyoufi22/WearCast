using WearCast.Api.Common.Helper;
using WearCast.Api.Common.Views;

namespace WearCast.Api.Features.DesignedProductManagement.CustomerDesigns.GetAllCustomerDesigns
{
    public class GetAllCustomerDesignsHandler(
        ApplicationDbContext context,
        IHttpContextAccessor httpContextAccessor
        ) : IRequestHandler<GetAllCustomerDesignsRequest, Result<PagingViewModel<GetAllCustomerDesignsResponse>>>
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<Result<PagingViewModel<GetAllCustomerDesignsResponse>>> Handle(GetAllCustomerDesignsRequest request, CancellationToken cancellationToken)
        {
            var customerId = httpContextAccessor.HttpContext?.User.GetCustomerId();

            if (customerId == null || customerId == 0)
            {
                return Result.Failure<PagingViewModel<GetAllCustomerDesignsResponse>>(
                    new Error("CustomerDesign.Unauthorized", "User is not authenticated as a valid customer.", StatusCodes.Status401Unauthorized));
            }

            var query = context.CustomerDesigns
                .AsNoTracking()
                .Where(d => d.CustomerId == customerId)
                .OrderByDescending(d => d.Id)
                .Select(d => new GetAllCustomerDesignsResponse(
                    d.Id,
                    d.DesignedProductId,
                    d.Name,
                    d.DesignedProduct.Name,
                    d.TotalPrice,
                    d.FrontImageUrl ?? d.BackImageUrl ?? d.RightImageUrl ?? d.LeftImageUrl,
                    d.CreatedOn
                ));

            var pagedResult = await PagingHelper.CreateAsync(query, request.PageIndex, request.PageSize);

            return Result.Success(pagedResult);
        }
    }
}
