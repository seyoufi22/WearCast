namespace WearCast.Api.Features.Sellers
{
    public static class SellerManagerErrors
    {
        public static readonly Error EmailInUse =
            new("SellerManager.EmailInUse",
                "The seller manager's email is already in use by another user.",
                 StatusCodes.Status409Conflict);

        public static readonly Error PhoneInUse =
            new("SellerManager.PhoneInUse",
                "The seller manager's phone number is already in use by another user.",
                StatusCodes.Status409Conflict);

        public static readonly Error EmailNotConfirmed =
            new("Manager.EmailNotConfirmed",
                "The manager's email address has not been confirmed.",
                StatusCodes.Status400BadRequest);
    }
}
