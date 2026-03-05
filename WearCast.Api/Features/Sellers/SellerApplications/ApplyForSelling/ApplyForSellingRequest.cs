namespace WearCast.Api.Features.Sellers.SellerApplications.ApplyForSelling
{
    public record ApplyForSellingRequest(
        string SellerManagerEmail,
        string SellerManagerFirstName,
        string SellerManagerLastName,
        string SellerManagerPhoneNumber,
        string SellerManagerPassword,
        string SellerManagerConfirmPassword,

        string SellerName,
        string SellerEmail,
        string SellerPhoneNumber,
        string SellerCommercialRegisterNumber,
        string SellerTaxIdNumber,
        string SellerDescription,
        IFormFile SellerLogo,

        string SellerState,
        string SellerCity,
        string SellerStreet,
        string SellerBuildingNumber
        ) : IRequest<Result>;
}
