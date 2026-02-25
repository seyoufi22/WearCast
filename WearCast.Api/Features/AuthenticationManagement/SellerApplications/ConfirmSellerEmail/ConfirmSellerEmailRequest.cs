namespace WearCast.Api.Features.AuthenticationManagement.SellerApplications.ConfirmSellerEmail
{
    public record ConfirmSellerEmailRequest(string Email, string Code) : IRequest<Result>;
}
