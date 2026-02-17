namespace WearCast.Api.Features.AuthenticationManagement.ResendConfirmEmail
{
    public class ResendConfirmEmailRequestValidator : AbstractValidator<ResendConfirmEmailRequest>
    {
        public ResendConfirmEmailRequestValidator() 
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();
        } 
    }
}
