namespace WearCast.Api.Features.DesignedProductManagement.DesignedProductReviews.DeleteDesignedProductReview
{
    public class DeleteDesignedProductReviewHandler(
        ApplicationDbContext context,
        IHttpContextAccessor httpContextAccessor
        ) : IRequestHandler<DeleteDesignedProductReviewRequest, Result>
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<Result> Handle(DeleteDesignedProductReviewRequest request, CancellationToken cancellationToken)
        {
            var user = _httpContextAccessor.HttpContext!.User;
            var customerId = user.GetCustomerId();

            bool isAdmin = user.IsSuperAdmin();

            var review = await _context.DesignedProductReviews
                .FirstOrDefaultAsync(r => r.Id == request.ReviewId, cancellationToken);

            if (review == null)
                return Result.Failure(DesignedProductReviewsErrors.ReviewNotFound);


            if (!isAdmin && review.CustomerId != customerId)
                return Result.Failure(new("DesignedProductReview.Forbidden", "You do not have permission to delete this review.", StatusCodes.Status403Forbidden));

            var product = await _context.DesignedProducts
                .FirstOrDefaultAsync(p => p.Id == review.DesignedProductId, cancellationToken);

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

            _context.DesignedProductReviews.Remove(review);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
