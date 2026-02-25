namespace WearCast.Api.Features.AuthenticationManagement.ConfirmEmail
{
    public class ConfirmEmailRequestValidator : AbstractValidator<ConfirmEmailRequest>
    {
        public ConfirmEmailRequestValidator() 
        {
            RuleFor(x => x.UserId)
                .NotEmpty();

            RuleFor(x => x.Code)
               .NotEmpty();
        }
    }
}
