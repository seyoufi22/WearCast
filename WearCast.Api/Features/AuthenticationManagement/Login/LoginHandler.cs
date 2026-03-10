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
                var (userRoles, userPermissions) = await GetUserRolesAndPermissions(user, cancellationToken);

                var profileClaims = await GetProfileClaimsAsync(user.Id, userRoles, cancellationToken);

                var (token, expiresIn) = _jwtProvider.GenerateToken(user, userRoles, userPermissions, profileClaims);

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

        private async Task<(IEnumerable<string> roles, IEnumerable<string> permissions)> GetUserRolesAndPermissions(ApplicationUser user, CancellationToken cancellationToken)
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            var userPermissions = await (from r in _context.Roles
                                         join p in _context.RoleClaims
                                         on r.Id equals p.RoleId
                                         where userRoles.Contains(r.Name!)
                                         select p.ClaimValue!)
                                        .Distinct()
                                        .ToListAsync(cancellationToken);

            return (userRoles, userPermissions);
        }
<<<<<<< HEAD
=======
<<<<<<< Updated upstream
=======
>>>>>>> Development

        private async Task<Dictionary<string, string>> GetProfileClaimsAsync(string userId, IEnumerable<string> roles, CancellationToken cancellationToken)
        {
            var claims = new Dictionary<string, string>();

            if (roles.Contains(DefaultRoles.Customer))
            {
                var customerId = await _context.Customers
                    .Where(c => c.UserId == userId)
                    .Select(c => (int?)c.Id)
                    .FirstOrDefaultAsync(cancellationToken);

                if (customerId != null)
                {
                    claims.Add("CustomerId", customerId.ToString()!);
                }
            }

            if (roles.Contains(DefaultRoles.Driver))
            {
                var driverId = await _context.Drivers
                    .Where(d => d.UserId == userId)
                    .Select(d => (int?)d.Id)
                    .FirstOrDefaultAsync(cancellationToken);

                if (driverId != null)
                {
                    claims.Add("DriverId", driverId.ToString()!);
                }
            }


<<<<<<< HEAD
            if (roles.Contains(DefaultRoles.Factory))
=======
            if (roles.Contains(DefaultRoles.FactoryManager))
>>>>>>> Development
            {
                var factoryManagerId = await _context.FactoryManagers
                    .Where(fm => fm.UserId == userId)
                    .Select(fm => (int?)fm.Id)
                    .FirstOrDefaultAsync(cancellationToken);

                if (factoryManagerId != null)
                {
                    claims.Add("FactoryManagerId", factoryManagerId.ToString());
                }
            }


<<<<<<< HEAD
            if (roles.Contains(DefaultRoles.Seller))
=======
            if (roles.Contains(DefaultRoles.SellerManager))
>>>>>>> Development
            {
                var sellerManagerId = await _context.SellerManagers
                    .Where(sm => sm.UserId == userId)
                    .Select(sm => (int?)sm.Id)
                    .FirstOrDefaultAsync(cancellationToken);

                if (sellerManagerId != null)
                {
                    claims.Add("SellerManagerId", sellerManagerId.ToString()!);
                }
            }


<<<<<<< HEAD
            if (roles.Contains(DefaultRoles.ShippingCompany))
=======
            if (roles.Contains(DefaultRoles.ShippingCompanyManager))
>>>>>>> Development
            {
                var shippingCompanyManagerId = await _context.ShippingCompanyManagers
                    .Where(scm => scm.UserId == userId)
                    .Select(scm => (int?)scm.Id)
                    .FirstOrDefaultAsync(cancellationToken);

                if (shippingCompanyManagerId != null)
                {
                    claims.Add("ShippingCompanyManagerId", shippingCompanyManagerId.ToString()!);
                }
            }

            return claims;
        }
<<<<<<< HEAD
=======
>>>>>>> Stashed changes
>>>>>>> Development
    }
}