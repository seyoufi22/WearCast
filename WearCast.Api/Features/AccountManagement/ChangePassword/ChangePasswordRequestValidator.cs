namespace WearCast.Api.Features.AccountManagement.ChangePassword
{
    public class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
    {
        public ChangePasswordRequestValidator()
        {
            RuleFor(x => x.CurrentPassword)
                .NotEmpty();

            RuleFor(x => x.NewPassword)
                .NotEmpty()
                .Matches(RegexPatterns.Password)
                .WithMessage("Password should be at least 8 digits and should contains Lowercase, NonAlphanumeric and Uppercase")
                .NotEqual(x => x.CurrentPassword)
                .WithMessage("New password can't be same as the current password");

            RuleFor(x => x.ConfirmNewPassword)
               .NotEmpty().WithMessage("Password confirmation is required.")
               .Equal(x => x.NewPassword).WithMessage("Password and confirmation password do not match.");
        }
    }
}
