namespace WearCast.Api.Features.Drivers
{
    public static class DriverErrors
    {
        public static readonly Error DuplicatedNationalId =
             new("Driver.DuplicatedNationalId",
                 "Another driver with the same national id already exists.",
                 StatusCodes.Status409Conflict);

        public static readonly Error NotFound =
            new("Driver.NotFound",
                "No driver found with This ID.",
                StatusCodes.Status404NotFound);

        public static readonly Error HasOutForDeliveryShipments =
            new("Driver.HasOutForDeliveryShipments",
                "Driver cannot be deactivated while shipments are out for delivery.",
                StatusCodes.Status409Conflict);

        public static readonly Error NotAvailable =
            new("Driver.NotAvailable",
                "This driver is currently not available.",
                StatusCodes.Status409Conflict);
    }
}
