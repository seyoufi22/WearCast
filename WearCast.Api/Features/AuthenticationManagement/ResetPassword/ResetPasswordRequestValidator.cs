namespace WearCast.Api.Features.AuthenticationManagement.ResetPassword
{
    public class ResetPasswordRequestValidator : AbstractValidator<ResetPasswordRequest>
    {
        public ResetPasswordRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Code)
                .NotEmpty();

            RuleFor(x => x.NewPassword)
                 .NotEmpty()
                 .Matches(RegexPatterns.Password)
                 .WithMessage("Password should be at least 8 digits and should contains Lowercase, NonAlphanumeric and Uppercase");

            RuleFor(x => x.ConfirmNewPassword)
               .NotEmpty().WithMessage("Password confirmation is required.")
               .Equal(x => x.NewPassword).WithMessage("Password and confirmation password do not match.");

        }
    }
}
