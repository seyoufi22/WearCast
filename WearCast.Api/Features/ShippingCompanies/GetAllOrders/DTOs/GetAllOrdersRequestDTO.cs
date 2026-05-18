using WearCast.Api.Common.Views;

namespace WearCast.Api.Features.ShippingCompanies.GetAllOrders.DTOs
{
    public class GetAllOrdersRequestDTO : IRequest<Result<PagingViewModel<GetAllOrdersResponseDTO>>>
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public SortBy SortBy { get; set; } = SortBy.Newest;
        public OrderStatus? OrderStatus { get; set; } = null;
        public OrderType? OrderType { get; set; } = null;
        public string? VendorCity { get; set; } = null;
        public ShipmentStatus? ShipmentStatus { get; set; } = null;
    }
    public class GetAllOrdersValidator : AbstractValidator<GetAllOrdersRequestDTO>
    {
        public GetAllOrdersValidator()
        {
            RuleFor(x => x.PageIndex)
                .GreaterThan(0).WithMessage("Page index must be greater than 0.");

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100).WithMessage("Page size must be between 1 and 100.");

            RuleFor(x => x.SortBy)
                .IsInEnum().WithMessage("Invalid sort option.");

            RuleFor(x => x.OrderStatus)
                .IsInEnum()
                .When(x => x.OrderStatus.HasValue).WithMessage("Invalid status value.");

            RuleFor(x => x.OrderType)
                .IsInEnum()
                .When(x => x.OrderType.HasValue).WithMessage("Invalid order type value.");

            RuleFor(x => x.VendorCity)
                .MaximumLength(50)
                .When(x => !string.IsNullOrEmpty(x.VendorCity))
                .WithMessage("Too long city name.");

            RuleFor(x => x.ShipmentStatus)
                .IsInEnum()
                .When(x => x.ShipmentStatus.HasValue).WithMessage("Invalid shipment status type value.");

        }
    }
}
