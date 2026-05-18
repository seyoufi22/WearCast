using WearCast.Api.Common.Views;
namespace WearCast.Api.Features.Shipments.Driver.GetAllShipments.DTOs
{
    public class GetAllDriverShipmentsRequestDTO : IRequest<Result<PagingViewModel<GetAllDriverShipmentsResponseDTO>>>
    {
        public int DriverId { get; set; }

        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 100;

        public SortBy SortBy { get; set; } = SortBy.Newest;
        public ShipmentStatus? ShipmentStatus { get; set; } = null;
        public string? DeliveryCity { get; set; } = null;
        public string? DeliveryStreet { get; set; } = null;
        public string? CustomerFirstName { get; set; } = null;
        public string? CustomerLastName { get; set; } = null;
    }
    public class GetAllDriverShipmentsValidator : AbstractValidator<GetAllDriverShipmentsRequestDTO>
    {
        public GetAllDriverShipmentsValidator()
        {
            RuleFor(x => x.DriverId)
            .GreaterThan(0)
            .WithMessage("Driver ID must be valid.");

            RuleFor(x => x.PageIndex)
           .GreaterThan(0)
           .WithMessage("Page index must be greater than 0.");

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100)
                .WithMessage("Page size must be between 1 and 100.");

            RuleFor(x => x.SortBy)
           .IsInEnum()
           .WithMessage("Invalid sort option.");

            RuleFor(x => x.ShipmentStatus)
                .IsInEnum()
                .When(x => x.ShipmentStatus.HasValue)
                .WithMessage("Invalid status value.");

            RuleFor(x => x.DeliveryCity)
                .MaximumLength(100).WithMessage("City name cannot exceed 100 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.DeliveryCity));

            RuleFor(x => x.DeliveryStreet)
                .MaximumLength(100).WithMessage("Street name cannot exceed 100 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.DeliveryStreet));

            RuleFor(x => x.CustomerFirstName)
                .MaximumLength(100).WithMessage("Customer first name cannot exceed 100 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.CustomerFirstName));

            RuleFor(x => x.CustomerLastName)
                .MaximumLength(100).WithMessage("Customer last name cannot exceed 100 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.CustomerLastName));
        }
    }
}
