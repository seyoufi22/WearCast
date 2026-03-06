namespace WearCast.Api.Features.Factories.CreateFactory
{
    public class CreateFactoryRequestValidator : AbstractValidator<CreateFactoryRequest>
    {
        public CreateFactoryRequestValidator()
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

            RuleFor(x => x.FactoryName)
                .NotEmpty().WithMessage("Factory name is required.")
                .MaximumLength(100).WithMessage("Factory name must not exceed 100 characters.");

            RuleFor(x => x.FactoryEmail)
                .NotEmpty().WithMessage("Factory Email is required.")
                .EmailAddress().WithMessage("Invalid email format.")
                .MaximumLength(256);

            RuleFor(x => x.FactoryPhoneNumber)
                .NotEmpty()
                .Matches(RegexPatterns.EgyptianPhoneNumber)
                .WithMessage("Invalid Egyptian phone number format.");

            RuleFor(x => x.FactoryCommercialRegisterNumber)
                .NotEmpty().WithMessage("Commercial register number is required.")
                .Matches(RegexPatterns.CommercialRegisterNumber)
                .WithMessage("Commercial register number must consist of 6 to 20 digits only.");

            RuleFor(x => x.FactoryTaxIdNumber)
                .NotEmpty().WithMessage("Tax ID number is required.")
                .Matches(RegexPatterns.TaxIdNumber)
                .WithMessage("Tax ID number must consist of exactly 9 digits.");

            RuleFor(x => x.FactoryDescription)
                .NotEmpty().WithMessage("Description is required.")
                .Length(20, 500).WithMessage("Description must be between 20 and 500 characters.");

            RuleFor(x => x.FactoryLogo)
                .NotNull()
                .IsValidImage();

            RuleFor(x => x.FactoryState).NotEmpty().MaximumLength(50);
            RuleFor(x => x.FactoryCity).NotEmpty().MaximumLength(50);
            RuleFor(x => x.FactoryStreet).NotEmpty().MaximumLength(200);
            RuleFor(x => x.FactoryBuildingNumber).NotEmpty().MaximumLength(20);
        }
    }
}
