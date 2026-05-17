namespace WearCast.Api.Features.Shipments.AdminAndManager.GetShipmentById.DTOs
{
    public class GetShipmentByIdRequestDTO : IRequest<Result<GetShipmentByIdResponseDTO>>
    {
        public GetShipmentByIdRequestDTO(int shipmentid)
        {
            ShipmentId = shipmentid;
        }
        public int ShipmentId { get; set; }
    }
    public class GetShipmentByIdValidator : AbstractValidator<GetShipmentByIdRequestDTO>
    {
        public GetShipmentByIdValidator()
        {
            RuleFor(x => x.ShipmentId)
                .GreaterThan(0)
                .WithMessage("Shipment ID must be valid.");
        }
    }
}