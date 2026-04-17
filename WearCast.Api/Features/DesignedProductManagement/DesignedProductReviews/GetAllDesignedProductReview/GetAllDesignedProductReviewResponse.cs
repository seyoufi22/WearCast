namespace WearCast.Api.Features.DesignedProductManagement.DesignedProductReviews.GetAllDesignedProductReview
{
    public record GetAllDesignedProductReviewResponse(
        int Id,
        int CustomerId,
        string CustomerName,
        string? CustomerImageUrl,
        int Rating,
        string? Comment,
        DateTime CreatedAt
        );
}
