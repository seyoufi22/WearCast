using WearCast.Api.Common.Views;
using WearCast.Api.Features.Shipments.AdminAndManager.GetAllShipments.DTOs;

namespace WearCast.Api.Features.Drivers.GetAllDrivers.DTOs
{
    public class GetAllDriversRequestDTO : IRequest<Result<PagingViewModel<GetAllDriversResponseDTO>>>
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 100;

        public SortBy SortBy { get; set; } = SortBy.Newest;
        public string? DriverFirstName { get; set; } = null;
        public string? DriverLastName { get; set; } = null;
        public string? DriverNationalId { get; set; } = null;
        public DeliveryVehicleType? VehicleType { get; set; }
        public DriverStatus? DriverStatus { get; set; }
        public string? DriverCity { get; set; }
    }
    public class GetAllDriversValidator : AbstractValidator<GetAllDriversRequestDTO>
    {
        public GetAllDriversValidator()
        {
            RuleFor(x => x.PageIndex)
                .GreaterThan(0)
                .WithMessage("Page index must be greater than 0.");

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100)
                .WithMessage("Page size must be between 1 and 100.");

            RuleFor(x => x.SortBy)
                .IsInEnum()
                .WithMessage("Invalid sort option.");

            RuleFor(x => x.DriverFirstName)
                .MaximumLength(100).WithMessage("Driver first name cannot exceed 100 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.DriverFirstName));

            RuleFor(x => x.DriverLastName)
                .MaximumLength(100).WithMessage("Driver last name cannot exceed 100 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.DriverLastName));


            RuleFor(x => x.DriverNationalId)
                .Length(14).WithMessage("National ID must be exactly 14 digits.")
                .Matches(@"^\d{14}$").WithMessage("National ID must contain only numbers.")
                .When(x => !string.IsNullOrWhiteSpace(x.DriverNationalId));

            RuleFor(x => x.DriverCity)
                .MaximumLength(100).WithMessage("City name cannot exceed 100 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.DriverCity));

            RuleFor(x => x.DriverStatus)
                .IsInEnum()
                .WithMessage("Invalid driver status.")
                .When(x => x.DriverStatus.HasValue);

            RuleFor(x => x.VehicleType)
                .IsInEnum()
                .WithMessage("Invalid Vehicle Type.")
                .When(x => x.VehicleType.HasValue);
        }
    }
}