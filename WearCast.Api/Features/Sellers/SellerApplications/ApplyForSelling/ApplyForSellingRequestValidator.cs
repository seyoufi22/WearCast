namespace WearCast.Api.Features.Sellers.SellerApplications.ApplyForSelling
{
    public class ApplyForSellingRequestValidator : AbstractValidator<ApplyForSellingRequest>
    {
        public ApplyForSellingRequestValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;


            RuleFor(x => x.SellerManagerEmail)
                .NotEmpty().WithMessage("Manager Email is required.")
                .EmailAddress().WithMessage("Invalid email format.")
                .MaximumLength(256).WithMessage("Email must not exceed 256 characters.");

            RuleFor(x => x.SellerManagerFirstName)
                .NotEmpty().WithMessage("First name is required.")
                .Length(3, 50).WithMessage("First name must be between 3 and 50 characters.");

            RuleFor(x => x.SellerManagerLastName)
                .NotEmpty().WithMessage("Last name is required.")
                .Length(3, 50).WithMessage("Last name must be between 3 and 50 characters.");

            RuleFor(x => x.SellerManagerPhoneNumber)
                .NotEmpty()
                .Matches(RegexPatterns.EgyptianPhoneNumber)
                .WithMessage("Invalid Egyptian phone number format.");

            RuleFor(x => x.SellerManagerPassword)
                .NotEmpty()
                .Matches(RegexPatterns.Password)
                .WithMessage("Password should be at least 8 characters and contain Lowercase, NonAlphanumeric, and Uppercase.");

            RuleFor(x => x.SellerManagerConfirmPassword)
               .NotEmpty().WithMessage("Password confirmation is required.")
               .Equal(x => x.SellerManagerPassword).WithMessage("Passwords do not match.");

            // 

            RuleFor(x => x.SellerName)
                .NotEmpty().WithMessage("Seller name is required.")
                .MaximumLength(100).WithMessage("Seller name must not exceed 100 characters.");

            RuleFor(x => x.SellerEmail)
                .NotEmpty().WithMessage("Seller Email is required.")
                .EmailAddress().WithMessage("Invalid email format.")
                .MaximumLength(256);

            RuleFor(x => x.SellerPhoneNumber)
                .NotEmpty()
                .Matches(RegexPatterns.EgyptianPhoneNumber)
                .WithMessage("Invalid Egyptian phone number format.");

            RuleFor(x => x.SellerCommercialRegisterNumber)
                .NotEmpty().WithMessage("Commercial register number is required.")
                .Matches(RegexPatterns.CommercialRegisterNumber)
                .WithMessage("Commercial register number must consist of 6 to 20 digits only.");

            RuleFor(x => x.SellerTaxIdNumber)
                .NotEmpty().WithMessage("Tax ID number is required.")
                .Matches(RegexPatterns.TaxIdNumber)
                .WithMessage("Tax ID number must consist of exactly 9 digits.");

            RuleFor(x => x.SellerDescription)
                .NotEmpty().WithMessage("Description is required.")
                .Length(20, 500).WithMessage("Description must be between 20 and 500 characters.");

            RuleFor(x => x.SellerLogo)
                .NotNull()
                .IsValidImage();

            RuleFor(x => x.SellerState).NotEmpty().MaximumLength(50);
            RuleFor(x => x.SellerCity).NotEmpty().MaximumLength(50);
            RuleFor(x => x.SellerStreet).NotEmpty().MaximumLength(200);
            RuleFor(x => x.SellerBuildingNumber).NotEmpty().MaximumLength(20);
        }
    }
}