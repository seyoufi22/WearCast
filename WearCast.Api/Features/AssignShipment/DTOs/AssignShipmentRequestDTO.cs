namespace WearCast.Api.Features.AssignShipment.DTOs
{
    public class AssignShipmentRequestDTO : IRequest<AssignShipmentResponseDTO>
    {
        public int ShipmentId { get; set; }
        public int DriverId { get; set; }
    }
}
