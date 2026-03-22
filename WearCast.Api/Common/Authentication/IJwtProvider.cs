namespace WearCast.Api.Authentication
{
    public interface IJwtProvider
    {
        (string token, int expiresIn) GenerateToken(
            ApplicationUser user,
            string role,
            IEnumerable<string> permissions,
            Dictionary<string, string>? profileClaims = null);
        string? ValidateToken(string Token);
    }
}
