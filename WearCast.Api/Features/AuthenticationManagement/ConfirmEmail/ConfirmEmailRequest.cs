namespace WearCast.Api.Features.AuthenticationManagement.ConfirmEmail
{
    public record ConfirmEmailRequest(
        string UserId,
        string Code
        ) : IRequest<Result>;
}
