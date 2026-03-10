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

            // 1. التعديل: بنجيب رول واحد بس
            var (role, userPermissions) = await GetUserRoleAndPermissions(user, cancellationToken);

            // 2. التعديل: لازم نجيب الـ IDs تاني عشان نحطها في التوكين الجديد
            var profileClaims = await GetProfileClaimsAsync(user.Id, role, cancellationToken);

            // 3. التعديل: بنبعت الرول الواحد والـ profileClaims للدالة
            var (newToken, expiresIn) = _jwtProvider.GenerateToken(user, role, userPermissions, profileClaims);

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

        // تم التعديل لإرجاع رول واحد (string)
        private async Task<(string role, IEnumerable<string> permissions)> GetUserRoleAndPermissions(ApplicationUser user, CancellationToken cancellationToken)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            var userRole = userRoles.FirstOrDefault() ?? string.Empty;

            var userPermissions = await (from r in _context.Roles
                                         join p in _context.RoleClaims
                                         on r.Id equals p.RoleId
                                         where r.Name == userRole
                                         select p.ClaimValue!)
                                        .Distinct()
                                        .ToListAsync(cancellationToken);

            return (userRole, userPermissions);
        }

        // تم إضافة الدالة دي عشان التوكين الجديد ميخسرش الـ Actor IDs
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
                    var driverId = await _context.Drivers
                        .Where(d => d.UserId == userId)
                        .Select(d => (int?)d.Id)
                        .FirstOrDefaultAsync(cancellationToken);

                    if (driverId != null) claims.Add("DriverId", driverId.ToString()!);
                    break;

                case DefaultRoles.FactoryManager:
                    var factoryManagerId = await _context.FactoryManagers
                        .Where(fm => fm.UserId == userId)
                        .Select(fm => (int?)fm.Id)
                        .FirstOrDefaultAsync(cancellationToken);

                    if (factoryManagerId != null) claims.Add("FactoryManagerId", factoryManagerId.ToString()!);
                    break;

                case DefaultRoles.SellerManager:
                    var sellerManagerId = await _context.SellerManagers
                        .Where(sm => sm.UserId == userId)
                        .Select(sm => (int?)sm.Id)
                        .FirstOrDefaultAsync(cancellationToken);

                    if (sellerManagerId != null) claims.Add("SellerManagerId", sellerManagerId.ToString()!);
                    break;

                case DefaultRoles.ShippingCompanyManager:
                    var shippingCompanyManagerId = await _context.ShippingCompanyManagers
                        .Where(scm => scm.UserId == userId)
                        .Select(scm => (int?)scm.Id)
                        .FirstOrDefaultAsync(cancellationToken);

                    if (shippingCompanyManagerId != null) claims.Add("ShippingCompanyManagerId", shippingCompanyManagerId.ToString()!);
                    break;
            }

            return claims;
        }
    }
}