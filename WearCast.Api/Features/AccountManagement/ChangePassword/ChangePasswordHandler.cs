using WearCast.Api.Entities.Identity;

namespace WearCast.Api.Features.AccountManagement.ChangePassword
{
    public class ChangePasswordHandler(
        IHttpContextAccessor httpContextAccessor,
        UserManager<ApplicationUser> userManager
        ) : IRequestHandler<ChangePasswordRequest, Result>
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager = userManager;

        public async Task<Result> Handle(ChangePasswordRequest request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User.GetUserId()!;

            var user = await _userManager.FindByIdAsync(userId);

            var result = await _userManager.ChangePasswordAsync(user!, request.CurrentPassword, request.NewPassword);

            if (result.Succeeded)
                return Result.Success();

            var error = result.Errors.First();

            return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
        }
    }
}
