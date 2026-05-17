using System.Security.Cryptography;
using RefreshTokenEntity = WearCast.Api.Entities.Identity.RefreshToken;

namespace WearCast.Api.Features.AuthenticationManagement.RefreshToken
{
    public class RefreshTokenHandler(
        IJwtProvider jwtProvider,
        UserManager<ApplicationUser> userManager,
        ApplicationDbContext context
        )
        : IRequestHandler<RefreshTokenRequest, Result<AuthResponse>>
    {
        private readonly IJwtProvider _jwtProvider = jwtProvider;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly ApplicationDbContext _context = context;

        private readonly int _refreshTokenExpiryDays = 30;

        public async Task<Result<AuthResponse>> Handle(RefreshTokenRequest request, CancellationToken cancellationToken)
        {
            var userId = _jwtProvider.ValidateToken(request.Token);

            if (userId is null)
                return Result.Failure<AuthResponse>(UserErrors.InvalidJwtToken);

            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
                return Result.Failure<AuthResponse>(UserErrors.InvalidJwtToken);

            if (user.IsDisabled)
                return Result.Failure<AuthResponse>(UserErrors.DisabledUser);

            if (user.LockoutEnd > DateTime.UtcNow)
                return Result.Failure<AuthResponse>(UserErrors.LockedUser);

            var userRefreshToken = user.RefreshTokens.SingleOrDefault(x => x.Token == request.RefreshToken && x.IsActive);

            if (userRefreshToken is null)
                return Result.Failure<AuthResponse>(UserErrors.InvalidRefreshToken);

            userRefreshToken.RevokedOn = DateTime.UtcNow;

            var role = await GetUserRole(user, cancellationToken);

            var profileClaims = await GetProfileClaimsAsync(user.Id, role, cancellationToken);

            var (newToken, expiresIn) = _jwtProvider.GenerateToken(user, role, profileClaims);

            var newRefreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);

            user.RefreshTokens.Add(new RefreshTokenEntity
            {
                Token = newRefreshToken,
                ExpiresOn = refreshTokenExpiration
            });

            await _userManager.UpdateAsync(user);

            var response = new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName, newToken, expiresIn, newRefreshToken, refreshTokenExpiration);

            return Result.Success(response);
        }

        private async Task<string> GetUserRole(ApplicationUser user, CancellationToken cancellationToken)
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            var userRole = userRoles.FirstOrDefault() ?? string.Empty;

            return userRole;
        }

        private async Task<Dictionary<string, string>> GetProfileClaimsAsync(string userId, string role, CancellationToken cancellationToken)
        {
            var claims = new Dictionary<string, string>();

            switch (role)
            {
                case DefaultRoles.Customer:
                    var customerId = await _context.Customers
                        .Where(c => c.UserId == userId)
                        .Select(c => (int?)c.Id)
                        .FirstOrDefaultAsync(cancellationToken);

                    if (customerId != null) claims.Add("CustomerId", customerId.ToString()!);
                    break;

                case DefaultRoles.Driver:
                    var driverInfo = await _context.Drivers
                        .Where(d => d.UserId == userId)
                        .Select(d => new { d.Id, d.ShippingCompanyId })
                        .FirstOrDefaultAsync(cancellationToken);

                    if (driverInfo != null)
                    {
                        claims.Add("DriverId", driverInfo.Id.ToString());
                        if (driverInfo.ShippingCompanyId != null)
                            claims.Add("ShippingCompanyId", driverInfo.ShippingCompanyId.ToString()!);
                    }
                    break;

                case DefaultRoles.FactoryManager:
                    var factoryInfo = await _context.FactoryManagers
                        .Where(fm => fm.UserId == userId)
                        .Select(fm => new { fm.Id, fm.FactoryId })
                        .FirstOrDefaultAsync(cancellationToken);

                    if (factoryInfo != null)
                    {
                        claims.Add("FactoryManagerId", factoryInfo.Id.ToString());
                        claims.Add("FactoryId", factoryInfo.FactoryId.ToString());
                    }
                    break;

                case DefaultRoles.SellerManager:
                    var sellerInfo = await _context.SellerManagers
                        .Where(sm => sm.UserId == userId)
                        .Select(sm => new { sm.Id, sm.SellerId })
                        .FirstOrDefaultAsync(cancellationToken);

                    if (sellerInfo != null)
                    {
                        claims.Add("SellerManagerId", sellerInfo.Id.ToString());
                        claims.Add("SellerId", sellerInfo.SellerId.ToString());
                    }
                    break;

                case DefaultRoles.ShippingCompanyManager:
                    var shippingInfo = await _context.ShippingCompanyManagers
                        .Where(scm => scm.UserId == userId)
                        .Select(scm => new { scm.Id, scm.ShippingCompanyId })
                        .FirstOrDefaultAsync(cancellationToken);

                    if (shippingInfo != null)
                    {
                        claims.Add("ShippingCompanyManagerId", shippingInfo.Id.ToString());
                        claims.Add("ShippingCompanyId", shippingInfo.ShippingCompanyId.ToString());
                    }
                    break;
            }

            return claims;
        }
    }
}