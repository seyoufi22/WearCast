namespace WearCast.Api.Features.ShippingCompanies.CreateShippingCompany
{
    public class CreateShippingCompanyRequestValidator : AbstractValidator<CreateShippingCompanyRequest>
    {
        public CreateShippingCompanyRequestValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;


            RuleFor(x => x.ManagerEmail)
                .NotEmpty().WithMessage("Manager Email is required.")
                .EmailAddress().WithMessage("Invalid email format.")
                .MaximumLength(256).WithMessage("Email must not exceed 256 characters.");

            RuleFor(x => x.ManagerFirstName)
                .NotEmpty().WithMessage("First name is required.")
                .Length(3, 50).WithMessage("First name must be between 3 and 50 characters.");

            RuleFor(x => x.ManagerLastName)
                .NotEmpty().WithMessage("Last name is required.")
                .Length(3, 50).WithMessage("Last name must be between 3 and 50 characters.");

            RuleFor(x => x.ManagerPhoneNumber)
                .NotEmpty()
                .Matches(RegexPatterns.EgyptianPhoneNumber)
                .WithMessage("Invalid Egyptian phone number format.");

            RuleFor(x => x.ManagerPassword)
                .NotEmpty()
                .Matches(RegexPatterns.Password)
                .WithMessage("Password should be at least 8 characters and contain Lowercase, NonAlphanumeric, and Uppercase.");

            RuleFor(x => x.ManagerConfirmPassword)
               .NotEmpty().WithMessage("Password confirmation is required.")
               .Equal(x => x.ManagerPassword).WithMessage("Passwords do not match.");

            // 

            RuleFor(x => x.CompanyName)
                .NotEmpty().WithMessage("ShippingCompany name is required.")
                .MaximumLength(100).WithMessage("ShippingCompany name must not exceed 100 characters.");

            RuleFor(x => x.CompanyEmail)
                .NotEmpty().WithMessage("ShippingCompany Email is required.")
                .EmailAddress().WithMessage("Invalid email format.")
                .MaximumLength(256);

            RuleFor(x => x.CompanyPhoneNumber)
                .NotEmpty()
                .Matches(RegexPatterns.EgyptianPhoneNumber)
                .WithMessage("Invalid Egyptian phone number format.");

            RuleFor(x => x.CompanyCommercialRegisterNumber)
                .NotEmpty().WithMessage("Commercial register number is required.")
                .Matches(RegexPatterns.CommercialRegisterNumber)
                .WithMessage("Commercial register number must consist of 6 to 20 digits only.");

            RuleFor(x => x.CompanyTaxIdNumber)
                .NotEmpty().WithMessage("Tax ID number is required.")
                .Matches(RegexPatterns.TaxIdNumber)
                .WithMessage("Tax ID number must consist of exactly 9 digits.");

            RuleFor(x => x.CompanyDescription)
                .NotEmpty().WithMessage("Description is required.")
                .Length(20, 500).WithMessage("Description must be between 20 and 500 characters.");

            RuleFor(x => x.CompanyLogo)
                .NotNull()
                .IsValidImage();

            RuleFor(x => x.CompanyState).NotEmpty().MaximumLength(50);
            RuleFor(x => x.CompanyCity).NotEmpty().MaximumLength(50);
            RuleFor(x => x.CompanyStreet).NotEmpty().MaximumLength(200);
            RuleFor(x => x.CompanyBuildingNumber).NotEmpty().MaximumLength(20);
        }
    }
}
