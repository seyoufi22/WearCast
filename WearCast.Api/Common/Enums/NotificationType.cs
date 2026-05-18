namespace WearCast.Api.Common.Enums
{
    public enum NotificationType
    {
        ShipmentUpdateStatus = 1,//get shipment by id
        DriverBecameNonActive = 2,//get all shipments
        DriverDeleted = 3,//get all drivers
        NewSellerApplication = 4,
        NewOrder = 5,//get order by id
        NewShipment = 6,//get shipment by id
        NewProduct = 7//get product by id
    }
}
