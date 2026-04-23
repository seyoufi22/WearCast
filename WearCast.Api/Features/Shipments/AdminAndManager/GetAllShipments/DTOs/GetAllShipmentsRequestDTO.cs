using WearCast.Api.Common.Views;
using WearCast.Api.Features.Shipments.Customer.GetAllShipments.DTOs;

namespace WearCast.Api.Features.Shipments.AdminAndManager.GetAllShipments.DTOs
{
    public class GetAllShipmentsRequestDTO : IRequest<Result<PagingViewModel<GetAllShipmentsResponseDTO>>>
    {

        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 100;

        public SortBy SortBy { get; set; } = SortBy.Newest;
        public bool? IsDeleted { get; set; }
        public ShipmentStatus? ShipmentStatus { get; set; } = null;

        public DateOnly? StartDate { get; set; } = null;
        public DateOnly? EndDate { get; set; } = null;

        public decimal? MinPrice { get; set; } = null;
        public decimal? MaxPrice { get; set; } = null;

        public int? MinNumberOfOrders { get; set; } = null;
        public int? MaxNumberOfOrders { get; set; } = null;

        public string? DeliveryState { get; set; } = null;
        public string? DeliveryCity { get; set; } = null;
        public string? DeliveryStreet { get; set; } = null;

        public string? DriverFirstName { get; set; } = null;
        public string? DriverLastName { get; set; } = null;
        public string? DriverNationalId { get; set; } = null;

        public string? CustomerFirstName { get; set; } = null;
        public string? CustomerLastName { get; set; } = null;
    }
    public class GetAllShipmentsValidator : AbstractValidator<GetAllShipmentsRequestDTO>
    {
        public GetAllShipmentsValidator()
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

            RuleFor(x => x.ShipmentStatus)
                .IsInEnum()
                .When(x => x.ShipmentStatus.HasValue)
                .WithMessage("Invalid status value.");

            RuleFor(x => x.MinPrice)
                .GreaterThanOrEqualTo(0)
                .When(x => x.MinPrice.HasValue)
                .WithMessage("Min price cannot be negative.");

            RuleFor(x => x.MaxPrice)
                .GreaterThanOrEqualTo(x => x.MinPrice ?? 0)
                .When(x => x.MaxPrice.HasValue)
                .WithMessage("Max price must be greater than min price.");

            RuleFor(x => x.MinNumberOfOrders)
                .GreaterThanOrEqualTo(0)
                .When(x => x.MinNumberOfOrders.HasValue)
                .WithMessage("Min number of orders cannot be negative.");

            RuleFor(x => x.MaxNumberOfOrders)
                .GreaterThanOrEqualTo(x => x.MinNumberOfOrders ?? 0)
                .When(x => x.MaxNumberOfOrders.HasValue)
                .WithMessage("Max number of orders must be greater than min number of orders.");

            RuleFor(x => x.EndDate)
                .GreaterThanOrEqualTo(x => x.StartDate)
                .When(x => x.StartDate.HasValue && x.EndDate.HasValue)
                .WithMessage("End date must be greater than or equal to start date.");

            RuleFor(x => x.DeliveryState)
                .MaximumLength(100).WithMessage("State name cannot exceed 100 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.DeliveryState));

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

        }
    }
}
