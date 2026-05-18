using System.Text.Json.Serialization;

namespace WearCast.Api.Features.Shipments.GetShipmentOrders.DTOs
{
    public class GetShipmentOrdersRequestDTO : IRequest<Result<List<GetShipmentOrdersResponseDTO>>>
    {
        [JsonIgnore]
        public int ShipmentId { get; set; }
        [JsonIgnore]
        public int? DriverId { get; set; }
        public OrderType? OrderType { get; set; }
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

            RuleFor(x => x.OrderType)
                .IsInEnum()
                .When(x => x.OrderType.HasValue)
                .WithMessage("Order type must be valid.");

            RuleFor(x => x.OrderStatus)
                .IsInEnum()
                .When(x => x.OrderStatus.HasValue)
                .WithMessage("Order Status must be valid.");
        }
    }
}
