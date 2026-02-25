namespace WearCast.Api.Features.AuthenticationManagement.RevokeRefreshToken
{
    public record RevokeRefreshTokenRequest(
        string Token,
        string RefreshToken
        ) : IRequest<Result>;
}
