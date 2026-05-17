using System.Security.Cryptography;
using RefreshTokenEntity = WearCast.Api.Entities.Identity.RefreshToken;

namespace WearCast.Api.Features.AuthenticationManagement.Login
{
    public class LoginHandler(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IJwtProvider jwtProvider,
        ApplicationDbContext context
        ) : IRequestHandler<LoginRequest, Result<AuthResponse>>
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
        private readonly IJwtProvider _jwtProvider = jwtProvider;
        private readonly ApplicationDbContext _context = context;

        private readonly int _refreshTokenExpiryDays = 30;

        public async Task<Result<AuthResponse>> Handle(LoginRequest request, CancellationToken cancellationToken)
        {
            if (await _userManager.FindByEmailAsync(request.Email) is not { } user)
                return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);

            if (user.IsDisabled)
                return Result.Failure<AuthResponse>(UserErrors.DisabledUser);

            var result = await _signInManager.PasswordSignInAsync(user, request.Password, false, true);

            if (result.Succeeded)
            {
                var role = await GetUserRole(user, cancellationToken);
                var profileClaims = await GetProfileClaimsAsync(user.Id, role, cancellationToken);

                var (token, expiresIn) = _jwtProvider.GenerateToken(user, role, profileClaims);

                var refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
                var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);

                user.RefreshTokens.Add(new RefreshTokenEntity
                {
                    Token = refreshToken,
                    ExpiresOn = refreshTokenExpiration
                });

                await _userManager.UpdateAsync(user);

                var response = new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName, token, expiresIn, refreshToken, refreshTokenExpiration);

                return Result.Success(response);
            }

            var error = result.IsNotAllowed
                ? UserErrors.EmailNotConfirmed
                : result.IsLockedOut
                ? UserErrors.LockedUser
                : UserErrors.InvalidCredentials;

            return Result.Failure<AuthResponse>(error);
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