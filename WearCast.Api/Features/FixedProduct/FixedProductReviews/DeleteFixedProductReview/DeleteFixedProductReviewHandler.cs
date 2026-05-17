namespace WearCast.Api.Features.FixedProduct.FixedProductReviews.DeleteFixedProductReview;

public class DeleteFixedProductReviewHandler(
    ApplicationDbContext context,
    IHttpContextAccessor httpContextAccessor
    ) : IRequestHandler<DeleteFixedProductReviewRequest, Result>
{
    private readonly ApplicationDbContext _context = context;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public async Task<Result> Handle(DeleteFixedProductReviewRequest request, CancellationToken cancellationToken)
    {
        var user = _httpContextAccessor.HttpContext!.User;
        var customerId = user.GetCustomerId();

        bool isAdmin = user.IsSuperAdmin() || user.IsCatalogAdmin() || user.IsCustomerServiceAdmin();

        var review = await _context.FixedProductReviews
            .FirstOrDefaultAsync(r => r.Id == request.ReviewId, cancellationToken);

        if (review == null)
            return Result.Failure(new("FixedProductReview.NotFound", "Review not found.", StatusCodes.Status404NotFound));

        if (!isAdmin && review.CustomerId != customerId)
            return Result.Failure(new("FixedProductReview.Forbidden", "You do not have permission to delete this review.", StatusCodes.Status403Forbidden));

        var product = await _context.FixedProducts
            .FirstOrDefaultAsync(p => p.Id == review.FixedProductId, cancellationToken);

        if (product != null)
        {
            if (product.ReviewCount <= 1)
            {
                product.AverageRating = 0m;
                product.ReviewCount = 0;
            }
            else
            {
                decimal totalScore = (product.AverageRating * product.ReviewCount) - review.Rating;
                product.ReviewCount--;

                decimal calculatedAverage = totalScore / product.ReviewCount;
                product.AverageRating = Math.Clamp(calculatedAverage, 0m, 5m);
            }
        }

        _context.FixedProductReviews.Remove(review);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}