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

    }
}
