namespace WearCast.Api.Features.ShippingCompanies.ShippingCompanyManagers.UpdateShippingCompanyManager
{
    public record UpdateShippingCompanyManagerRequest(
        string FirstName,
        string LastName,
        string PhoneNumber,
        int? ProvidedManagerId = null
        ) : IRequest<Result>;
}
