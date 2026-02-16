namespace WearCast.Api.Features.AuthenticationManagement.ResendConfirmEmail
{
    public record ResendConfirmEmailRequest(
        string Email
        ) : IRequest<Result<ResendConfirmEmailResponse>>;
}
