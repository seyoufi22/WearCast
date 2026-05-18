namespace WearCast.Api.Features.ShippingCompanies.ShippingCompanyManagers
{
    public static class ShippingCompanyManagerErrors
    {
        public static readonly Error NotFound =
            new("ShippingCompanyManager.NotFound", "The specified shipping company manager does not exist.", StatusCodes.Status404NotFound);

        public static readonly Error AlreadyDeleted =
            new("ShippingCompanyManager.AlreadyDeleted", "This manager is already deleted.", StatusCodes.Status400BadRequest);
        public static readonly Error CannotDeleteYourself =
            new("ShippingCompanyManager.CannotDeleteYourself",
            "You cannot delete your own manager account.",
            StatusCodes.Status400BadRequest);

        public static readonly Error CurrentManagerNotFound =
            new("ShippingCompanyManager.CurrentManagerNotFound",
            "The current user's shipping company manager profile could not be found.",
            StatusCodes.Status404NotFound);

        public static readonly Error UnauthorizedToDeleteManager =
            new("ShippingCompanyManager.Unauthorized",
            "You are not authorized to delete a manager from a different shipping company.",
            StatusCodes.Status403Forbidden);

        public static readonly Error CannotDeleteLastManager =
            new("ShippingCompanyManager.CannotDeleteLastManager",
            "Cannot delete the last manager of this shipping company. A shipping company must have at least one active manager.",
            StatusCodes.Status400BadRequest);
    }
}
