namespace WearCast.Api.Features.Sellers.SellerManagers.UpdateSellerManager
{
    public record UpdateSellerManagerRequest(
        string FirstName,
        string LastName,
        string PhoneNumber,
        int? ProvidedManagerId = null
        ) : IRequest<Result>;
}
