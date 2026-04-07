namespace WearCast.Api.Features.Factories.UpdateFactory
{
    public class UpdateFactoryRequestValidator : AbstractValidator<UpdateFactoryRequest>
    {
        public UpdateFactoryRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Factory name is required.")
                .MaximumLength(100).WithMessage("Factory name must not exceed 100 characters.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Factory email is required.")
                .EmailAddress().WithMessage("Invalid email format.")
                .MaximumLength(256);

            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .Matches(RegexPatterns.EgyptianPhoneNumber)
                .WithMessage("Invalid Egyptian phone number format.");

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

            RuleFor(x => x.Address)
                .NotNull().WithMessage("Address details are required.")
                .SetValidator(new AddressDtoValidator());
        }
    }
}
