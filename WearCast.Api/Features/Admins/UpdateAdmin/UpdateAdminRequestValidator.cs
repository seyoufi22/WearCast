namespace WearCast.Api.Features.Admins.UpdateAdmin
{
    public class UpdateAdminRequestValidator : AbstractValidator<UpdateAdminRequest>
    {
        public UpdateAdminRequestValidator()
        {
            RuleFor(x => x.AdminId).NotEmpty().WithMessage("Admin ID is required.");
            RuleFor(x => x.Email).NotEmpty().EmailAddress();

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .Length(3, 100).WithMessage("First name must be between 3 and 100 characters.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .Length(3, 100).WithMessage("Last name must be between 3 and 100 characters.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .Matches(RegexPatterns.EgyptianPhoneNumber)
                .WithMessage("Invalid Egyptian phone number format.");

            RuleFor(x => x.Role).IsInEnum().WithMessage("Invalid admin role.");
        }
    }
}
