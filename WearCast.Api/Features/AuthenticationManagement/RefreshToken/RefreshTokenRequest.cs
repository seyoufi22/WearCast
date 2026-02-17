namespace WearCast.Api.Features.AuthenticationManagement.RefreshToken
{
    public record RefreshTokenRequest(
        string Token,
        string RefreshToken
        ) : IRequest<Result<AuthResponse>>;
}
