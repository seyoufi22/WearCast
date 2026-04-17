namespace WearCast.Api.Features.DesignedProductManagement.DesignedProductReviews
{
    public static class DesignedProductReviewsErrors
    {
        public static readonly Error Forbidden =
            new("DesignedProductReview.Forbidden", "You do not have permission to delete this review.", StatusCodes.Status403Forbidden);

        public static readonly Error ReviewNotFound =
            new("DesignedProductReview.ReviewNotFound", "Review not found.", StatusCodes.Status404NotFound);
    }
}
