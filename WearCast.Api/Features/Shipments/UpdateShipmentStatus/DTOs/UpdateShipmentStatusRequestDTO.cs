using System.Text.Json.Serialization;

namespace WearCast.Api.Features.Shipments.UpdateShipmentStatus.DTOs
{
    public class UpdateShipmentStatusRequestDTO : IRequest<Result>
    {
        [JsonIgnore]
        public int ShipmentId { get; set; }
        public ShipmentStatus NewStatus { get; set; }
    }
    public class UpdateShipmentStatusValidator : AbstractValidator<UpdateShipmentStatusRequestDTO>
    {
        public UpdateShipmentStatusValidator()
        {
            RuleFor(x => x.ShipmentId)
                .GreaterThan(0).WithMessage("Shipment ID must be valid.");

            RuleFor(x => x.NewStatus)
                .IsInEnum().WithMessage("Invalid shipment status.");
        }
    }
}
