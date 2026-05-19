namespace WearCast.Api.Features.AuthenticationManagement.ConfirmEmail
{
    public record ConfirmEmailRequest(
        string Email,
        string Code
        ) : IRequest<Result>;
}
