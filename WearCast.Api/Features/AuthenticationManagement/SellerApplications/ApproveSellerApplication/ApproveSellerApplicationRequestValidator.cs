namespace WearCast.Api.Features.AuthenticationManagement.SellerApplications.ApproveSellerApplication
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
