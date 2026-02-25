namespace WearCast.Api.Features.AuthenticationManagement.SellerApplications.ResendSellerConfirmEmail
{
    public record ResendConfirmSellerEmailRequest(string Email) : IRequest<Result<ResendConfirmSellerEmailResponse>>;
}
