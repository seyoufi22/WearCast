namespace WearCast.Api.Features.AuthenticationManagement.SellerApplications.ResendSellerConfirmEmail
{
    public class ResendConfirmSellerEmailRequestValidator : AbstractValidator<ResendConfirmSellerEmailRequest>
    {
        public ResendConfirmSellerEmailRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();
        }
    }
}
