namespace WearCast.Api.Features.Sellers
{
    public static class SellerApplicationErrors
    {

        public static readonly Error ApplicationPending =
            new("SellerApplication.Pending",
                "An application with this email is currently pending review",
                StatusCodes.Status400BadRequest);

        public static readonly Error ApplicationNotFound =
            new("SellerApplication.NotFound",
                "Application not found",
                StatusCodes.Status404NotFound);

        public static readonly Error ApplicationNotPending =
            new("SellerApplication.NotPending",
                "Only pending applications can be approved",
                StatusCodes.Status400BadRequest);

        public static readonly Error EmailNotConfirmed =
            new("SellerApplication.EmailNotConfirmed", "Email is not confirmed", StatusCodes.Status401Unauthorized);

        public static readonly Error DublicatedConfirmation =
            new("SellerApplication.DublicatedConfirmation", "Email already confirmed", StatusCodes.Status400BadRequest);

        public static readonly Error InvalidCode =
            new("SellerApplication.InvalidCode", "Invalid code", StatusCodes.Status400BadRequest);

        public static readonly Error ApproveFailed =
            new("Application.ApproveFailed", "An error occurred while approving the application.", StatusCodes.Status500InternalServerError);
    }
}
