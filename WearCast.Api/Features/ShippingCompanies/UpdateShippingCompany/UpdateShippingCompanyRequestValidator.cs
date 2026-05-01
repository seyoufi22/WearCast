namespace WearCast.Api.Features.ShippingCompanies.UpdateShippingCompany
{
    public class UpdateShippingCompanyRequestValidator : AbstractValidator<UpdateShippingCompanyRequest>
    {
        public UpdateShippingCompanyRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("ShippingCompany name is required.")
                .MaximumLength(100).WithMessage("ShippingCompany name must not exceed 100 characters.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("ShippingCompany Email is required.")
                .EmailAddress().WithMessage("Invalid email format.")
                .MaximumLength(256);

            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .Matches(RegexPatterns.EgyptianPhoneNumber)
                .WithMessage("Invalid Egyptian phone number format.");

            RuleFor(x => x.CommercialRegisterNumber)
                .NotEmpty().WithMessage("Commercial register number is required.")
                .MaximumLength(20).WithMessage("Commercial register number must not exceed 20 characters.");

            RuleFor(x => x.TaxIdNumber)
                .NotEmpty().WithMessage("Tax ID number is required.")
                .MaximumLength(20).WithMessage("Tax ID number must not exceed 20 characters.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters.");

            RuleFor(x => x.DeliveryFee)
                .GreaterThanOrEqualTo(0).WithMessage("Delivery fee cannot be negative.");

            RuleFor(x => x.Address)
                .NotNull().WithMessage("Address details are required.")
                .SetValidator(new AddressDtoValidator());
        }
    }
}
