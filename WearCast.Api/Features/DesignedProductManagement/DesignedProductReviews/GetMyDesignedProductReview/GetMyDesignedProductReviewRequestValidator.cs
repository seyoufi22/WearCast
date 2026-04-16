namespace WearCast.Api.Features.DesignedProductManagement.DesignedProductReviews.GetMyDesignedProductReview
{
    public class GetMyDesignedProductReviewRequestValidator : AbstractValidator<GetMyDesignedProductReviewRequest>
    {
        public GetMyDesignedProductReviewRequestValidator()
        {
            RuleFor(x => x.DesignedProductId)
                .GreaterThan(0).WithMessage("Invalid Product ID.");
        }
    }
}
