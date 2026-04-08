namespace WearCast.Api.Features.Sellers.SellerApplications.ResendSellerManagerConfirmEmail
{
    public class ResendConfirmSellerManagerEmailRequestValidator : AbstractValidator<ResendConfirmSellerManagerEmailRequest>
    {
        public ResendConfirmSellerManagerEmailRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();
        }
    }
}
