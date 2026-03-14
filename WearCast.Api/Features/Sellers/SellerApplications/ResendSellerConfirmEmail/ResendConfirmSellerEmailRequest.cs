namespace WearCast.Api.Features.Sellers.SellerApplications.ResendSellerConfirmEmail
{
    public record ResendConfirmSellerEmailRequest(string Email) : IRequest<Result<ResendConfirmSellerEmailResponse>>;
}
