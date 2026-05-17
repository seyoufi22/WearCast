namespace WearCast.Api.Features.FixedProduct.FixedProductReviews.GetAllFixedProductReview
{
    public record GetAllFixedProductReviewResponse(
        int Id,
        int CustomerId,
        string CustomerName,
        string? CustomerImageUrl,
        int Rating,
        string? Comment,
        DateTime CreatedAt
    );
}