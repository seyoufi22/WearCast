namespace WearCast.Api.Features.AuthenticationManagement.RevokeRefreshToken
{
    public class RevokeRefreshTokenRequestValidator : AbstractValidator<RevokeRefreshTokenRequest>
    {
        public RevokeRefreshTokenRequestValidator() 
        {
            RuleFor(x => x.Token).NotEmpty();
            RuleFor(x => x.RefreshToken).NotEmpty();
        }
    }
}
