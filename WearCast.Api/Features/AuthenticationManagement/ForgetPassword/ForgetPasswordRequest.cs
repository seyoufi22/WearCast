namespace WearCast.Api.Features.AuthenticationManagement.ForgetPassword
{
    public record ForgetPasswordRequest(
        string Email
        ) : IRequest<Result<ForgetPasswordResponse>>;
}
