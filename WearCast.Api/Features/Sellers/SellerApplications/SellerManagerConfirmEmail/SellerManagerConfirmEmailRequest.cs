namespace WearCast.Api.Features.Sellers.SellerApplications.SellerManagerConfirmEmail
{
    public record SellerManagerConfirmEmailRequest(string Email, string Code) : IRequest<Result>;
}
