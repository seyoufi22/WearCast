using System.Text.Json.Serialization;

namespace WearCast.Api.Features.Shipments.Driver.UnAssignShipment.DTOs
{
    public class UnAssignShipmentRequestDTO : IRequest<Result>
    {
        [JsonIgnore]
        public int ShipmentId { get; set; }
        [JsonIgnore]
        public string? UpdaterId { get; set; } = null;
        [JsonIgnore]
        public bool IsAdmin { get; set; } = false;
    }
    public class UnAssignShipmentValidator : AbstractValidator<UnAssignShipmentRequestDTO>
    {
        public UnAssignShipmentValidator()
        {
            RuleFor(x => x.ShipmentId)
                .GreaterThan(0).WithMessage("Shipment ID must be valid.");
        }
    }
}
