namespace WearCast.Api.Features.DesignedProductManagement.DesignedProductReviews.GetMyDesignedProductReview
{
    public class GetMyDesignedProductReviewHandler(
        ApplicationDbContext context,
        IHttpContextAccessor httpContextAccessor
        ) : IRequestHandler<GetMyDesignedProductReviewRequest, Result<GetMyDesignedProductReviewResponse>>
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<Result<GetMyDesignedProductReviewResponse>> Handle(GetMyDesignedProductReviewRequest request, CancellationToken cancellationToken)
        {
            var customerId = _httpContextAccessor.HttpContext!.User.GetCustomerId();

            var review = await _context.DesignedProductReviews
                .Where(r => r.DesignedProductId == request.DesignedProductId && r.CustomerId == customerId)
                .Select(r => new GetMyDesignedProductReviewResponse(
                    r.Id,
                    r.Rating,
                    r.Comment,
                    r.CreatedOn
                ))
                .FirstOrDefaultAsync(cancellationToken);
            if (review == null)
                return Result.Success<GetMyDesignedProductReviewResponse>(null!);

            return Result.Success(review);
        }
    }
}
