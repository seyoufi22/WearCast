namespace WearCast.Api.Features.DesignedProductManagement.DesignedProductReviews.UpsertDesignedProductReview
{
    public class UpsertDesignedProductReviewRequestValidator : AbstractValidator<UpsertDesignedProductReviewRequest>
    {
        public UpsertDesignedProductReviewRequestValidator()
        {
            RuleFor(x => x.DesignedProductId)
                .GreaterThan(0).WithMessage("Invalid Product ID.");

            RuleFor(x => x.Rating)
                .InclusiveBetween(1, 5).WithMessage("Rating must be between 1 and 5 stars.");

            RuleFor(x => x.Comment)
                .MaximumLength(500).WithMessage("Comment cannot exceed 500 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.Comment));
        }
    }
}
