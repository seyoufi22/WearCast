using WearCast.Api.Common.Views;

namespace WearCast.Api.Features.Drivers.GetDriverOrders.DTOs
{
    public class GetDriverOrdersRequestDTO : IRequest<Result<PagingViewModel<GetDriverOrdersResponseDTO>>>
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int DriverId {  get; set; }
        public SortBy SortBy { get; set; } = SortBy.Newest;
        public OrderStatus? OrderStatus { get; set; } = null;
        public OrderType? OrderType { get; set; } = null;
        public string? VendorCity { get; set; } = null;
    }
    public class GetDriverOrdersValidator : AbstractValidator<GetDriverOrdersRequestDTO>
    {
        public GetDriverOrdersValidator()
        {
            RuleFor(x => x.PageIndex)
                .GreaterThan(0).WithMessage("Page index must be greater than 0.");

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100).WithMessage("Page size must be between 1 and 100.");

            RuleFor(x => x.SortBy)
                .IsInEnum().WithMessage("Invalid sort option.");

            RuleFor(x => x.DriverId)
              .GreaterThan(0).WithMessage("Driver Id must be valid.");


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

        }
    }
}
