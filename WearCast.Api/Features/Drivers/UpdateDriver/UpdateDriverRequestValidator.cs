namespace WearCast.Api.Features.Drivers.UpdateDriver
{
    public class UpdateDriverRequestValidator : AbstractValidator<UpdateDriverRequest>
    {
        public UpdateDriverRequestValidator()
        {
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

            RuleFor(x => x.NationalId)
                .NotEmpty()
                .Length(14).WithMessage("National ID must be exactly 14 digits.")
                .Matches("^[0-9]*$").WithMessage("National ID must contain digits only.");

            RuleFor(x => x.VehicleType)
                .IsInEnum().WithMessage("Invalid vehicle type selected.");

            RuleFor(x => x.VehiclePlateNumber)
                .NotEmpty()
                .WithMessage("Vehicle plate number is required.")
                .When(x => x.VehicleType != DeliveryVehicleType.Bicycle)
                .MaximumLength(20)
                .WithMessage("Plate number must not exceed 20 characters.");

            RuleFor(x => x.VehiclePlateNumber)
                .MaximumLength(20).WithMessage("Plate number must not exceed 20 characters.")
                .When(x => !string.IsNullOrEmpty(x.VehiclePlateNumber));

            RuleFor(x => x.Address)
                .NotNull().WithMessage("Address details are required.")
                .SetValidator(new AddressDtoValidator());
        }
    }
}
