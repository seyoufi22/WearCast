namespace WearCast.Api.Features.Drivers.Dashboard.DTOs
{
    public class DriverDashboardResponseDTO
    {
        public int PendingOrders { get; set; }
        public int PickedUpOrders { get; set; }
        public int AssignedShipments { get; set; }
        public int PickingUpShipments { get; set; }
        public int OutForDeliveryShipments { get; set; }
        public int DeliveredShipments { get; set; }
        public DriverStatus DriverStatus { get; set; }
        public DeliveryVehicleType DeliveryVehicleType { get; set; }

    }
}
