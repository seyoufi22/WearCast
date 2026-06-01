namespace WearCast.Api.Features.Sellers
{
    public static class SellerErrors
    {
        public static readonly Error NameInUse =
          new Error("Application.SellerNameInUse",
              "This Brand Name is already registered.",
              StatusCodes.Status400BadRequest);

        public static readonly Error CommercialRegisterInUse =
           new Error("Application.CommercialRegisterInUse",
               "Commercial Register is already in use.",
               StatusCodes.Status400BadRequest);

        public static readonly Error TaxIdInUse =
           new Error("Application.TaxIdInUse",
               "Tax ID is already in use.",
               StatusCodes.Status400BadRequest);

        public static readonly Error AlreadyExists =
            new("SellerApplication.Approved",
                "There is already a registered seller with this email",
                StatusCodes.Status409Conflict);


        public static readonly Error EmailInUse =
            new("Seller.EmailInUse",
                "Seller email is already in use.",
                StatusCodes.Status400BadRequest);

        public static readonly Error PhoneInUse =
            new("Seller.PhoneInUse",
            "Seller phone number is already in use.",
            StatusCodes.Status400BadRequest);

        public static readonly Error SellerNotFound =
            new("Seller.NotFound",
            "The specified seller was not found.",
            StatusCodes.Status404NotFound);

        public static readonly Error ManagerNotFound =
             new("SellerManager.NotFound",
             "The specified manager was not found.",
             StatusCodes.Status404NotFound);

        public static readonly Error CannotDeleteYourself =
             new("SellerManager.CannotDeleteYourself",
             "You cannot delete your own manager account.",
             StatusCodes.Status400BadRequest);

        public static readonly Error CurrentManagerNotFound =
             new("SellerManager.CurrentManagerNotFound",
             "The current user's manager profile could not be found.",
             StatusCodes.Status404NotFound);

        public static readonly Error UnauthorizedToDeleteManager =
             new("SellerManager.Unauthorized",
             "You are not authorized to delete a manager from a different seller.",
             StatusCodes.Status403Forbidden);

        public static readonly Error CannotDeleteLastManager =
            new("SellerManager.CannotDeleteLastManager",
            "Cannot delete the last manager of this seller. A seller must have at least one active manager.",
            StatusCodes.Status400BadRequest);
    }
}
