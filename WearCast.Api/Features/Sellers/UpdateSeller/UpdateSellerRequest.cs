namespace WearCast.Api.Features.Sellers.UpdateSeller
{
    public record UpdateSellerRequest(
        string Name,
        string Email,
        string PhoneNumber,
        string CommercialRegisterNumber,
        string TaxIdNumber,
        string Description,
        AddressDto Address,
        int? ProvidedSellerId = null
        ) : IRequest<Result>;
}
