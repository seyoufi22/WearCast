namespace WearCast.Api.Features.Shipments.GetShipmentOrders.DTOs
{
    public class GetShipmentOrdersRequestDTO : IRequest<Result<List<GetShipmentOrdersResponseDTO>>>
    {
        public int ShipmentId { get; set; }
        public int? DriverId { get; set; }
        public OrderType? orderType { get; set; }
        public OrderStatus? OrderStatus { get; set; }
    }
    public class GetShipmentOrdersValidator : AbstractValidator<GetShipmentOrdersRequestDTO>
    {
        public GetShipmentOrdersValidator()
        {
            RuleFor(x => x.ShipmentId)
                .GreaterThan(0)
                .WithMessage("Shipment ID must be valid.");

            RuleFor(x => x.DriverId)
                .GreaterThan(0)
                .When(x => x.DriverId.HasValue)
                .WithMessage("Driver ID must be valid.");

            RuleFor(x => x.orderType)
                .IsInEnum()
                .When(x => x.orderType.HasValue)
                .WithMessage("Order type must be valid.");

            RuleFor(x => x.OrderStatus)
                .IsInEnum()
                .When(x => x.OrderStatus.HasValue)
                .WithMessage("Order Status must be valid.");
        }
    }
}
