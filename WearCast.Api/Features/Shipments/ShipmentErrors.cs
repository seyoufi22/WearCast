namespace WearCast.Api.Features.Shipments
{
    public static class ShipmentErrors
    {
        public static readonly Error NotFound =
           new("Shipment.NotFound",
               "No shipment found with This ID",
               StatusCodes.Status404NotFound);

        public static readonly Error AlreadyAssigned =
           new("Shipment.AlreadyAssigned",
               "This shipment is already assigned to a driver or delivered.",
               StatusCodes.Status409Conflict);

    }
}
