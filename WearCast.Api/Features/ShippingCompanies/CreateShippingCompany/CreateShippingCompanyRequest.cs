namespace WearCast.Api.Features.ShippingCompanies.CreateShippingCompany
{
    public record CreateShippingCompanyRequest(
          string ManagerEmail,
          string ManagerFirstName,
          string ManagerLastName,
          string ManagerPhoneNumber,
          string ManagerPassword,
          string ManagerConfirmPassword,

          string CompanyName,
          string CompanyEmail,
          string CompanyPhoneNumber,
          string CommercialRegisterNumber,
          string TaxIdNumber,
          string Description,
          decimal DeliveryFee,
          IFormFile CompanyLogo,

          string CompanyState,
          string CompanyCity,
          string CompanyStreet,
          string CompanyBuildingNumber
      ) : IRequest<Result<CreateShippingCompanyResponse>>;
}
