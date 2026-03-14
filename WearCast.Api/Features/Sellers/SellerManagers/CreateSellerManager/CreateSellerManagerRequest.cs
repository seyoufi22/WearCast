namespace WearCast.Api.Features.Sellers.SellerManagers.CreateSellerManager
{
    public record CreateSellerManagerRequest(
        string Email,
        string FirstName,
        string LastName,
        string PhoneNumber,
        string Password,
        string ConfirmPassword,

        int SellerId
        ) : IRequest<Result>;
}
