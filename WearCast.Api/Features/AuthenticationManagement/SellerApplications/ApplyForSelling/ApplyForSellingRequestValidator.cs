namespace WearCast.Api.Features.AuthenticationManagement.SellerApplications.ApplyForSelling
{
    public class ApplyForSellingRequestValidator : AbstractValidator<ApplyForSellingRequest>
    {
        public ApplyForSellingRequestValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .Length(2, 50).WithMessage("First name must be between 2 and 50 characters.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .Length(2, 50).WithMessage("Last name must be between 2 and 50 characters.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required.")
                .Matches(RegexPatterns.EgyptianPhoneNumber)
                .WithMessage("Invalid Egyptian phone number format.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .Matches(RegexPatterns.Password)
                 .WithMessage("Password should be at least 8 digits and should contains Lowercase, NonAlphanumeric and Uppercase");

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage("Password confirmation is required.")
                .Equal(x => x.Password).WithMessage("Password and confirmation password do not match.");

            RuleFor(x => x.SellerName)
                .NotEmpty().WithMessage("Brand name is required.")
                .MaximumLength(100).WithMessage("Brand name must not exceed 100 characters.");

            RuleFor(x => x.CommercialRegisterNumber)
                .NotEmpty().WithMessage("Commercial register number is required.")
                .Matches(RegexPatterns.CommercialRegisterNumber)
                .WithMessage("Commercial register number must consist of 6 to 20 digits only.");

            RuleFor(x => x.TaxIdNumber)
                .NotEmpty().WithMessage("Tax ID number is required.")
                .Matches(RegexPatterns.TaxIdNumber)
                .WithMessage("Tax ID number must consist of exactly 9 digits.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .Length(20, 500).WithMessage("Description must be between 20 and 500 characters.");

            RuleFor(x => x.Logo)
                .NotNull().WithMessage("Store logo is required.")
                .Must(file => file.Length > 0).WithMessage("Uploaded file is empty.");

            RuleFor(x => x.State)
                .NotEmpty().WithMessage("State is required.")
                .MaximumLength(50).WithMessage("State name must not exceed 50 characters.");

            RuleFor(x => x.City)
                .NotEmpty().WithMessage("City is required.")
                .MaximumLength(50).WithMessage("City name must not exceed 50 characters.");

            RuleFor(x => x.Street)
                .NotEmpty().WithMessage("Street is required.")
                .MaximumLength(200).WithMessage("Street name must not exceed 200 characters.");

            RuleFor(x => x.BuildingNumber)
                .NotEmpty().WithMessage("Building number is required.")
                .MaximumLength(20).WithMessage("Building number must not exceed 20 characters.");
        }
    }
}
