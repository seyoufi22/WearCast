namespace WearCast.Api.Features.ShippingCompanies
{
    public static class ShippingCompanyErrors
    {
        public static readonly Error CompanyNotFound =
            new Error("ShippingCompany.NotFound", "The specified shipping company does not exist.", StatusCodes.Status404NotFound);
    }
}
