using WearCast.Api.Entities.FixedProduct; 
using WearCast.Api.Entities.Order;        


namespace WearCast.Api.Features.FixedProduct.FixedProductReviews.UpsertFixedProductReview;

public class UpsertFixedProductReviewHandler(
    ApplicationDbContext context,
    IHttpContextAccessor httpContextAccessor
    ) : IRequestHandler<UpsertFixedProductReviewRequest, Result<UpsertFixedProductReviewResponse>>
{
    private readonly ApplicationDbContext _context = context;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public async Task<Result<UpsertFixedProductReviewResponse>> Handle(UpsertFixedProductReviewRequest request, CancellationToken cancellationToken)
    {
        var customerId = _httpContextAccessor.HttpContext?.User.GetCustomerId();

        var product = await _context.FixedProducts
            .FirstOrDefaultAsync(p => p.Id == request.FixedProductId, cancellationToken);

        if (product == null)
            return Result.Failure<UpsertFixedProductReviewResponse>(new("FixedProductReview.ProductNotFound", "Product does not exist.", StatusCodes.Status404NotFound));
        var productColorsIds = await _context.FixedProductColors
            .Where(c => c.ProductId == request.FixedProductId)
            .Select(c => c.Id)
            .ToListAsync(cancellationToken);
        var hasPurchased = await context.Set<FixedProductOrderItem>()
            .AnyAsync(item =>
                item.Order.CustomerId == customerId!.Value &&
                item.Order.Status == OrderStatus.PickedUp &&
                productColorsIds.Contains(item.FixedColorId),
            cancellationToken);

        if (!hasPurchased)
            return Result.Failure<UpsertFixedProductReviewResponse>(new("FixedProductReview.NotPurchased", "You can only review products you have purchased and picked Up.", StatusCodes.Status403Forbidden));

        var existingReview = await _context.FixedProductReviews
            .FirstOrDefaultAsync(r => r.CustomerId == customerId
                                   && r.FixedProductId == request.FixedProductId,
                                 cancellationToken);

        FixedProductReview targetReview;

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

            var newReview = new FixedProductReview
            {
                CustomerId = customerId!.Value,
                FixedProductId = request.FixedProductId,
                Rating = request.Rating,
                Comment = request.Comment
            };

            _context.FixedProductReviews.Add(newReview);

            targetReview = newReview;
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success(new UpsertFixedProductReviewResponse(targetReview.Id));
    }
}