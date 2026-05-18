namespace WearCast.Api.Common.Enums
{
    public enum NotificationType
    {
        ShipmentUpdateStatus = 1,//get shipment by id [customer]
        DriverDeActivated = 2,//get all shipments [shipping company manager]
        DriverDeleted = 3,//get all shipments [shipping company manager]
        ShipmentUnAssigned = 4, //get shipment by id [shipping company manager]
        ShipmentAssigned = 5, //get driver shipment by id [driver]
        ShipmentReady=6,//get shipment by id [shipping company manager/customer]

    }
}
