namespace WearCast.Api.Features.Sellers.SellerApplications.ResendSellerManagerConfirmEmail
{
    public record ResendConfirmSellerManagerEmailRequest(string Email) : IRequest<Result<ResendConfirmSellerManagerEmailResponse>>;
}
