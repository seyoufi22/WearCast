using WearCast.Api.Entities.Order;

namespace WearCast.Api.Features.DesignedProductManagement.DesignedProductReviews.UpsertDesignedProductReview
{

    public class UpsertDesignedProductReviewHandler(
        ApplicationDbContext context,
        IHttpContextAccessor httpContextAccessor
        ) : IRequestHandler<UpsertDesignedProductReviewRequest, Result<UpsertDesignedProductReviewResponse>>
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        public async Task<Result<UpsertDesignedProductReviewResponse>> Handle(UpsertDesignedProductReviewRequest request, CancellationToken cancellationToken)
        {
            var customerId = _httpContextAccessor.HttpContext?.User.GetCustomerId();


            var product = await _context.DesignedProducts
                .FirstOrDefaultAsync(p => p.Id == request.DesignedProductId, cancellationToken);

            if (product == null)
                return Result.Failure<UpsertDesignedProductReviewResponse>(new("DesignedProductReview.ProductNotFound", "Product does not exist.", StatusCodes.Status404NotFound));

            var hasPurchased = await context.Set<CustomerDesignedOrderItem>()
                .AnyAsync(item =>
                    item.Order.CustomerId == customerId.Value &&
                    item.Order.Status == OrderStatus.PickedUp &&
                    item.DesignedProductId == request.DesignedProductId,
                cancellationToken);

            if (!hasPurchased)
                return Result.Failure<UpsertDesignedProductReviewResponse>(new("DesignedProductReview.NotPurchased", "You can only review products you have purchased and picked Up.", StatusCodes.Status403Forbidden));

            var existingReview = await _context.DesignedProductReviews
                .FirstOrDefaultAsync(r => r.CustomerId == customerId
                                       && r.DesignedProductId == request.DesignedProductId,
                                     cancellationToken);

            DesignedProductReview targetReview;

            if (existingReview != null)
            {
                if (existingReview.Rating != request.Rating)
                {
                    decimal totalScore = (product.AverageRating * product.ReviewCount) - existingReview.Rating + request.Rating;
                    decimal calculatedAverage = totalScore / product.ReviewCount;

                    product.AverageRating = Math.Clamp(calculatedAverage, 0m, 5m);
                }

                existingReview.Rating = request.Rating;
                existingReview.Comment = request.Comment;

                targetReview = existingReview;
            }
            else
            {
                decimal totalScore = (product.AverageRating * product.ReviewCount) + request.Rating;
                product.ReviewCount++;

                decimal calculatedAverage = totalScore / product.ReviewCount;
                product.AverageRating = Math.Clamp(calculatedAverage, 0m, 5m);

                var newReview = new DesignedProductReview
                {
                    CustomerId = customerId.Value,
                    DesignedProductId = request.DesignedProductId,
                    Rating = request.Rating,
                    Comment = request.Comment
                };

                _context.DesignedProductReviews.Add(newReview);

                targetReview = newReview;
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success(new UpsertDesignedProductReviewResponse(targetReview.Id));

        }
    }
}
