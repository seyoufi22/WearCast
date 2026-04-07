namespace WearCast.Api.Features.ShippingCompanies.ShippingCompanyManagers
{
    public static class ShippingCompanyManagerErrors
    {
        public static readonly Error NotFound =
            new("ShippingCompanyManager.NotFound", "The specified shipping company manager does not exist.", StatusCodes.Status404NotFound);
    }
}
