namespace WearCast.Api.Features.AuthenticationManagement.SellerApplications.ConfirmSellerEmail
{
    public class ConfirmSellerEmailRequestValidator : AbstractValidator<ConfirmSellerEmailRequest>
    {
        public ConfirmSellerEmailRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty();

            RuleFor(x => x.Code)
               .NotEmpty();
        }
    }
}
