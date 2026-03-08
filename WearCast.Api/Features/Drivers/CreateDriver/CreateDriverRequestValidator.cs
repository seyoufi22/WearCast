namespace WearCast.Api.Features.Drivers.CreateDriver
{
    public class CreateDriverRequestValidator : AbstractValidator<CreateDriverRequest>
    {
        public CreateDriverRequestValidator()
        {

            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(x => x.Email)
                 .NotEmpty().WithMessage("Email is required.")
                 .EmailAddress().WithMessage("Invalid email format.")
                 .MaximumLength(256).WithMessage("Email must not exceed 256 characters.");

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

            RuleFor(x => x.Password)
                .NotEmpty()
                .Matches(RegexPatterns.Password)
                .WithMessage("Password should be at least 8 digits and should contains Lowercase, NonAlphanumeric and Uppercase");

            RuleFor(x => x.ConfirmPassword)
               .NotEmpty().WithMessage("Password confirmation is required.")
               .Equal(x => x.Password).WithMessage("Password and confirmation password do not match.");

            RuleFor(x => x.State)
                  .NotEmpty().WithMessage("State is required.")
                  .MaximumLength(50).WithMessage("State must not exceed 50 characters.");

            RuleFor(x => x.City)
                .NotEmpty().WithMessage("City is required.")
                .MaximumLength(50).WithMessage("City must not exceed 50 characters.");

            RuleFor(x => x.Street)
                .NotEmpty().WithMessage("Street is required.")
                .MaximumLength(200).WithMessage("Street must not exceed 200 characters.");

            RuleFor(x => x.BuildingNumber)
                .NotEmpty().WithMessage("Building number is required.")
                .MaximumLength(20).WithMessage("Building number must not exceed 20 characters.");


            RuleFor(x => x.NationalId)
                .NotEmpty()
                .Length(14)
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

            RuleFor(x => x.ProfileImage)
                .NotNull()
                .IsValidImage();

            RuleFor(x => x.ShippingCompanyId)
               .NotEmpty().WithMessage("Shipping Company Id is required.");
        }
    }
}
