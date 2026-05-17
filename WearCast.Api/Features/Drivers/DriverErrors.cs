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

        public static readonly Error HasActiveShipments =
            new("Driver.HasActiveShipments",
                "This driver cannot be deactivated because he has active shipments.",
                StatusCodes.Status409Conflict);

        public static readonly Error NotAvailable =
            new("Driver.NotAvailable",
                "This driver is currently not available.",
                StatusCodes.Status409Conflict);

        public static readonly Error UnAuthorized =
          new("Driver.UnAuthorized",
              "You are not authorized to perform this action",
              StatusCodes.Status403Forbidden);
    
        public static readonly Error DeleteActiveDriver =
          new("Driver.DeleteActiveDriver",
              "This driver cannot be deleted because he has active shipments.",
              StatusCodes.Status409Conflict);
    }
}
