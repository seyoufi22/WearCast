using System.Text.Json.Serialization;

namespace WearCast.Api.Features.Shipments.Driver.UpdateShipmentStatus.DTOs
{
    public class UpdateShipmentStatusRequestDTO : IRequest<Result>
    {
        [JsonIgnore]
        public int ShipmentId { get; set; }
        [JsonIgnore]
        public string? UpdaterId { get; set; } = null;
        [JsonIgnore]
        public bool IsAdmin { get; set; } = false;
        public ShipmentStatus NewStatus { get; set; }
        public string? DeliveryCode { get; set; } = null;
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
