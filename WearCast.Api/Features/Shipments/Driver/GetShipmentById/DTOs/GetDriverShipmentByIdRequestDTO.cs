namespace WearCast.Api.Features.Shipments.Driver.GetShipmentById.DTOs
{
    public class GetDriverShipmentByIdRequestDTO: IRequest<Result<GetDriverShipmentByIdResponseDTO>>
    {
        public GetDriverShipmentByIdRequestDTO(int shipmentid,int driverid)
        {
            ShipmentId = shipmentid;
            DriverId = driverid;
        }

        public int ShipmentId { get; set; }
        public int DriverId { get; set; }
    }
    public class GetDriverShipmentByIdValidator : AbstractValidator<GetDriverShipmentByIdRequestDTO>
    {
        public GetDriverShipmentByIdValidator()
        {
            RuleFor(x => x.ShipmentId)
                .GreaterThan(0)
                .WithMessage("Shipment ID must be valid.");

            RuleFor(x => x.DriverId)
                .GreaterThan(0)
                .WithMessage("Driver ID must be valid.");
        }
    }
}
