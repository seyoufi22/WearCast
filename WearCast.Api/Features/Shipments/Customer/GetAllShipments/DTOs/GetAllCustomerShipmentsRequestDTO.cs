using WearCast.Api.Common.Views;

namespace WearCast.Api.Features.Shipments.Customer.GetAllShipments.DTOs
{
    public class GetAllCustomerShipmentsRequestDTO : IRequest<Result<PagingViewModel<GetAllCustomerShipmentsResponseDTO>>>
    {
        public int CustomerId { get; set; }

        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 100;

        public SortBy SortBy { get; set; } = SortBy.Newest;
        public ShipmentStatus? ShipmentStatus { get; set; } = null;

        public decimal? MinPrice { get; set; } = null;
        public decimal? MaxPrice { get; set; } = null;

        public string? DeliveryCity { get; set; } = null;
        public string? DeliveryStreet { get; set; } = null;
    }
    public class GetAllCustomerShipmentsValidator : AbstractValidator<GetAllCustomerShipmentsRequestDTO>
    {
        public GetAllCustomerShipmentsValidator()
        {
            RuleFor(x => x.CustomerId)
            .GreaterThan(0)
            .WithMessage("Customer ID must be valid.");

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

            RuleFor(x => x.DeliveryCity)
              .MaximumLength(100).WithMessage("City name cannot exceed 100 characters.")
              .When(x => !string.IsNullOrWhiteSpace(x.DeliveryCity));

            RuleFor(x => x.DeliveryStreet)
                .MaximumLength(100).WithMessage("Street name cannot exceed 100 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.DeliveryStreet));
        }
    }
}
