namespace WearCast.Api.Features.ShippingCompanies.UpdateShippingCompany
{
    public record UpdateShippingCompanyRequest(
        string Name,
        string Email,
        string PhoneNumber,
        string CommercialRegisterNumber,
        string TaxIdNumber,
        string Description,
        decimal DeliveryFee,
        AddressDto Address,
        int? ProvidedCompanyId = null
        ) : IRequest<Result>;
}
