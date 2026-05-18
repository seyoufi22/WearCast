namespace WearCast.Api.Common.Enums
{
    public enum NotificationType
    {
        ShipmentUpdateStatus = 1,//get shipment by id [customer]
        DriverDeActivated = 2,//get all shipments [shipping company manager]
        DriverDeleted = 3,//get all shipments [shipping company manager]
        NewSellerApplication = 4,// get SellerApplication by id
        NewOrder = 5,//get order by id
        NewShipment = 6,//get shipment by id
        NewProduct = 7,//get product by id
        ShipmentUnAssigned = 8, //get shipment by id [shipping company manager]
        ShipmentAssigned = 9, //get driver shipment by id [driver]
        ShipmentReady = 10,//get shipment by id [shipping company manager/customer]
    }
}
