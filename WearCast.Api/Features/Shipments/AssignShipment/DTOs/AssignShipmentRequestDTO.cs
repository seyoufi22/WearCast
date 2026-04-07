using System.Text.Json.Serialization;

namespace WearCast.Api.Features.Shipments.AssignShipment.DTOs
{
    public class AssignShipmentRequestDTO : IRequest<Result>
    {
        [JsonIgnore]
        public int ShipmentId { get; set; }
        public int DriverId { get; set; }
    }
    public class AssignShipmentValidator : AbstractValidator<AssignShipmentRequestDTO>
    {
        public AssignShipmentValidator()
        {
            RuleFor(x => x.ShipmentId)
                .GreaterThan(0).WithMessage("Shipment ID must be valid.");

            RuleFor(x => x.DriverId)
                .GreaterThan(0).WithMessage("Driver ID must be valid.");
        }
    }

}
