namespace WearCast.Api.Features.DesignedProductManagement.DesignedProductReviews.DeleteDesignedProductReview
{
    public class DeleteDesignedProductReviewRequestValidator : AbstractValidator<DeleteDesignedProductReviewRequest>
    {
        public DeleteDesignedProductReviewRequestValidator()
        {
            RuleFor(x => x.ReviewId)
                .GreaterThan(0).WithMessage("Invalid Review ID.");
        }
    }
}
