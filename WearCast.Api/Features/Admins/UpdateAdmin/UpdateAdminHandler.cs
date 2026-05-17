using WearCast.Api.Features.AuthenticationManagement;

namespace WearCast.Api.Features.Admins.UpdateAdmin
{
    public class UpdateAdminHandler(
        UserManager<ApplicationUser> userManager,
        ILogger<UpdateAdminHandler> logger,
        IHttpContextAccessor httpContextAccessor
    ) : IRequestHandler<UpdateAdminRequest, Result>
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly ILogger<UpdateAdminHandler> _logger = logger;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<Result> Handle(UpdateAdminRequest request, CancellationToken cancellationToken)
        {
            var currentUser = _httpContextAccessor.HttpContext?.User;

            var currentAdminId = currentUser?.GetAdminId();
            bool isSuperAdmin = currentUser != null && currentUser.IsSuperAdmin();

            string targetAdminId = isSuperAdmin ? request.AdminId : currentAdminId;

            if (string.IsNullOrEmpty(targetAdminId))
            {
                return Result.Failure(new("Admin.Unauthorized", "Unable to identify the admin.", StatusCodes.Status401Unauthorized));
            }

            var admin = await _userManager.FindByIdAsync(targetAdminId);
            if (admin == null)
                return Result.Failure(new("Admin.NotFound", "Admin account not found.", StatusCodes.Status404NotFound));

            bool isPhoneNumberDuplicated = await _userManager.Users
               .AnyAsync(x => x.Id != targetAdminId && x.PhoneNumber == request.PhoneNumber, cancellationToken);

            if (isPhoneNumberDuplicated)
            {
                return Result.Failure(UserErrors.DublicatedPhoneNumber);
            }

            admin.FirstName = request.FirstName;
            admin.LastName = request.LastName;
            admin.PhoneNumber = request.PhoneNumber;

            var updateResult = await _userManager.UpdateAsync(admin);
            if (!updateResult.Succeeded)
            {
                var errors = string.Join(", ", updateResult.Errors.Select(e => e.Description));
                _logger.LogError("Failed to update Admin details: {Errors}", errors);
                return Result.Failure(new("Admin.UpdateFailed", "Failed to update admin details.", StatusCodes.Status500InternalServerError));
            }

            var currentRoles = await _userManager.GetRolesAsync(admin);
            var newRoleName = request.Role.ToString();

            if (!currentRoles.Contains(newRoleName))
            {
                if (!isSuperAdmin)
                {
                    return Result.Failure(new("Admin.RoleChangeForbidden", "You do not have permission to change roles.", StatusCodes.Status403Forbidden));
                }

                var addResult = await _userManager.AddToRoleAsync(admin, newRoleName);
                if (!addResult.Succeeded)
                {
                    _logger.LogError("Failed to add new role {Role} for Admin {AdminId}", newRoleName, admin.Id);
                    return Result.Failure(new("Admin.RoleUpdateFailed", "Failed to assign new privileges.", StatusCodes.Status500InternalServerError));
                }

                var rolesToRemove = currentRoles.Where(r => r != newRoleName).ToList();
                if (rolesToRemove.Any())
                {
                    var removeResult = await _userManager.RemoveFromRolesAsync(admin, rolesToRemove);

                    if (!removeResult.Succeeded)
                    {
                        await _userManager.RemoveFromRoleAsync(admin, newRoleName);

                        _logger.LogError("Failed to remove old roles for Admin {AdminId}. Rolled back new role addition.", admin.Id);
                        return Result.Failure(new("Admin.RoleUpdateFailed", "System error while updating roles. No changes were saved.", StatusCodes.Status500InternalServerError));
                    }
                }
            }

            return Result.Success();
        }
    }
}