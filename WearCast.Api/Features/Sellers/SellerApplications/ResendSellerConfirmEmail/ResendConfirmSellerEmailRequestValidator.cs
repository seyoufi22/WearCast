namespace WearCast.Api.Features.Sellers.SellerApplications.ResendSellerConfirmEmail
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
