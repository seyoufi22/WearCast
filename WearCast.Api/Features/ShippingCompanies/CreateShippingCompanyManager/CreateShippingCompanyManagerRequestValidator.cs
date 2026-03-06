namespace WearCast.Api.Features.ShippingCompanies.CreateShippingCompanyManager
{
    public class CreateShippingCompanyManagerRequestValidator : AbstractValidator<CreateShippingCompanyManagerRequest>
    {
        public CreateShippingCompanyManagerRequestValidator()
        {
            RuleFor(x => x.Email)
               .NotEmpty().WithMessage("Email is required.")
               .EmailAddress().WithMessage("Invalid email format.")
              .MaximumLength(256).WithMessage("Email must not exceed 256 characters.");

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .Length(3, 50).WithMessage("First name must be between 3 and 50 characters.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .Length(3, 50).WithMessage("Last name must be between 3 and 50 characters.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .Matches(RegexPatterns.EgyptianPhoneNumber)
                .WithMessage("Invalid Egyptian phone number format.");

            RuleFor(x => x.Password)
                .NotEmpty()
                .Matches(RegexPatterns.Password)
                .WithMessage("Password should be at least 8 characters and contain Lowercase, NonAlphanumeric, and Uppercase.");

            RuleFor(x => x.ConfirmPassword)
               .NotEmpty().WithMessage("Password confirmation is required.")
               .Equal(x => x.Password).WithMessage("Passwords do not match.");

            RuleFor(x => x.ShippingCompanyId)
               .NotEmpty().WithMessage("Shipping Company Id is required.");
        }
    }
}
