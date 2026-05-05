namespace WearCast.Api.Features.FixedProduct.FixedProductReviews.GetMyFixedProductReview;

public class GetMyFixedProductReviewHandler(
    ApplicationDbContext context,
    IHttpContextAccessor httpContextAccessor
    ) : IRequestHandler<GetMyFixedProductReviewRequest, Result<GetMyFixedProductReviewResponse>>
{
    private readonly ApplicationDbContext _context = context;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public async Task<Result<GetMyFixedProductReviewResponse>> Handle(GetMyFixedProductReviewRequest request, CancellationToken cancellationToken)
    {
        var customerId = _httpContextAccessor.HttpContext!.User.GetCustomerId();

        var review = await _context.FixedProductReviews
            .Where(r => r.FixedProductId == request.FixedProductId && r.CustomerId == customerId)
            .Select(r => new GetMyFixedProductReviewResponse(
                r.Id,
                r.Rating,
                r.Comment,
                r.CreatedOn
            ))
            .FirstOrDefaultAsync(cancellationToken);

        if (review == null)
            return Result.Success<GetMyFixedProductReviewResponse>(null!);

        return Result.Success(review);
    }
}