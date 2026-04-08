namespace WearCast.Api.Features.Factories.UpdateFactory
{
    public record UpdateFactoryRequest(
        string Name,
        string Email,
        string PhoneNumber,
        string CommercialRegisterNumber,
        string TaxIdNumber,
        string Description,
        AddressDto Address,
        int? ProvidedFactoryId = null
        ) : IRequest<Result>;
}
