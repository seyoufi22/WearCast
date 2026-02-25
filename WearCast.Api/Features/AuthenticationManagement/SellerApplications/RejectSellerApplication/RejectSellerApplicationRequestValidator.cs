namespace WearCast.Api.Features.AuthenticationManagement.SellerApplications.RejectSellerApplication
{
    public class RejectSellerApplicationRequestValidator : AbstractValidator<RejectSellerApplicationRequest>
    {
        public RejectSellerApplicationRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.RejectionReason)
                .NotEmpty();
        }
    }
}
