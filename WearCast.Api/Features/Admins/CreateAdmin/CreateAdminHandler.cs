namespace WearCast.Api.Features.Admins.CreateAdmin
{
    public class CreateAdminHandler(
        UserManager<ApplicationUser> userManager,
        ILogger<CreateAdminHandler> logger
    ) : IRequestHandler<CreateAdminRequest, Result<CreateAdminResponse>>
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly ILogger<CreateAdminHandler> _logger = logger;

        public async Task<Result<CreateAdminResponse>> Handle(CreateAdminRequest request, CancellationToken cancellationToken)
        {
            var existingUser = await _userManager.Users
                 .Where(x => x.Email == request.Email || x.PhoneNumber == request.PhoneNumber)
                 .Select(x => new { x.Email, x.PhoneNumber })
                 .FirstOrDefaultAsync(cancellationToken);

            if (existingUser != null)
            {
                if (existingUser.Email!.Equals(request.Email?.Trim(), StringComparison.OrdinalIgnoreCase))
                    return Result.Failure<CreateAdminResponse>(new("Admin.EmailExists", "This email is already registered.", 400));

                return Result.Failure<CreateAdminResponse>(new("Admin.PhoneExists", "This phone number is already registered.", 400));
            }

            var newAdmin = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
                EmailConfirmed = true
            };

            var createResult = await _userManager.CreateAsync(newAdmin, request.Password);
            if (!createResult.Succeeded)
            {
                var errors = string.Join(", ", createResult.Errors.Select(e => e.Description));
                _logger.LogError("Failed to create Admin: {Errors}", errors);
                return Result.Failure<CreateAdminResponse>(new("Admin.CreationFailed", "Failed to create user account.", 400));
            }

            string roleName = request.Role.ToString();

            try
            {
                var roleResult = await _userManager.AddToRoleAsync(newAdmin, roleName);

                if (!roleResult.Succeeded)
                {
                    await _userManager.DeleteAsync(newAdmin);
                    var errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                    _logger.LogError("Failed to assign Admin role. User deleted. Errors: {Errors}", errors);
                    return Result.Failure<CreateAdminResponse>(new("Admin.RoleFailed", "Failed to assign administrative privileges.", 500));
                }
            }
            catch (Exception ex)
            {
                await _userManager.DeleteAsync(newAdmin);
                _logger.LogError(ex, "System exception thrown while assigning role. Rolling back user creation for {Email}.", newAdmin.Email);
                return Result.Failure<CreateAdminResponse>(new("Admin.RoleException", "A system error occurred while assigning privileges.", 500));
            }

            return Result.Success(new CreateAdminResponse(newAdmin.Id));
        }
    }
}