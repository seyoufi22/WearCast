namespace WearCast.Api.Features.ShippingCompanies.Dashboard.DTOs
{
    public class ShippingCompanyDashboardResponseDTO
    {
        public int PendingOrders { get; set; }
        public int PickedUpOrders { get; set; }
        public int PendingShipments { get; set; } 
        public int UnassignedShipments { get; set; } 
        public int AssignedShipments { get; set; } 
        public int PickingUpShipments { get; set; }
        public int OutForDeliveryShipments { get; set; }
        public int DeliveredShipments { get; set; }
        public int TotalDrivers { get; set; }
        public int ActiveDrivers { get; set; } 
        public int InactiveDrivers { get; set; }
        public double AverageDeliveryTimeInHours { get; set; }
        public int NumberOfManagers { get; set; }
    }
}
