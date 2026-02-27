namespace WearCast.Api.Features.Sellers.SellerApplications.ApproveSellerApplication
{
    public class ApproveSellerApplicationRequestValidator : AbstractValidator<ApproveSellerApplicationRequest>
    {
        public ApproveSellerApplicationRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();
        }
    }
}
