namespace WearCast.Api.Features.Sellers.SellerApplications
{
    public static class SellerApplicationsErrors
    {
        public static readonly Error NotFound =
            new("SellerApplication.NotFound",
                "The requested seller application was not found.",
                StatusCodes.Status404NotFound);
    }
}
