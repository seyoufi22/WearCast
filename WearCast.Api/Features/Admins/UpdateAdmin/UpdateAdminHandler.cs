using WearCast.Api.Features.AuthenticationManagement;

namespace WearCast.Api.Features.Admins.UpdateAdmin
{
    public class UpdateAdminHandler(
        UserManager<ApplicationUser> userManager,
        ILogger<UpdateAdminHandler> logger
    ) : IRequestHandler<UpdateAdminRequest, Result>
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly ILogger<UpdateAdminHandler> _logger = logger;

        public async Task<Result> Handle(UpdateAdminRequest request, CancellationToken cancellationToken)
        {
            var admin = await _userManager.FindByIdAsync(request.AdminId);
            if (admin == null)
                return Result.Failure(new("Admin.NotFound", "Admin account not found.", 404));

            bool isPhoneNumberDuplicated = await _userManager.Users
               .AnyAsync(x => x.Id != request.AdminId && x.PhoneNumber == request.PhoneNumber, cancellationToken);

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
                return Result.Failure(new("Admin.UpdateFailed", "Failed to update admin details.", 500));
            }

            var currentRoles = await _userManager.GetRolesAsync(admin);
            var newRoleName = request.Role.ToString();

            if (!currentRoles.Contains(newRoleName))
            {
                var addResult = await _userManager.AddToRoleAsync(admin, newRoleName);
                if (!addResult.Succeeded)
                {
                    _logger.LogError("Failed to add new role {Role} for Admin {AdminId}", newRoleName, admin.Id);
                    return Result.Failure(new("Admin.RoleUpdateFailed", "Failed to assign new privileges.", 500));
                }

                var rolesToRemove = currentRoles.Where(r => r != newRoleName).ToList();
                if (rolesToRemove.Any())
                {
                    var removeResult = await _userManager.RemoveFromRolesAsync(admin, rolesToRemove);

                    if (!removeResult.Succeeded)
                    {
                        await _userManager.RemoveFromRoleAsync(admin, newRoleName);

                        _logger.LogError("Failed to remove old roles for Admin {AdminId}. Rolled back new role addition.", admin.Id);
                        return Result.Failure(new("Admin.RoleUpdateFailed", "System error while updating roles. No changes were saved.", 500));
                    }
                }
            }

            return Result.Success();
        }
    }
}