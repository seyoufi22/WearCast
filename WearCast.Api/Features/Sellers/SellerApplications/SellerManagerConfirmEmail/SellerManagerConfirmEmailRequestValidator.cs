namespace WearCast.Api.Features.Sellers.SellerApplications.SellerManagerConfirmEmail
{
    public class SellerManagerConfirmEmailRequestValidator : AbstractValidator<SellerManagerConfirmEmailRequest>
    {
        public SellerManagerConfirmEmailRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty();

            RuleFor(x => x.Code)
               .NotEmpty();
        }
    }
}
