using WearCast.Api.Features.AuthenticationManagement;


namespace WearCast.Api.Features.Admins.CreateSuperAdmin
{
    public class CreateSuperAdminHandler(
        UserManager<ApplicationUser> userManager
    ) : IRequestHandler<CreateSuperAdminRequest, Result>
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;

        public async Task<Result> Handle(CreateSuperAdminRequest request, CancellationToken cancellationToken)
        {
            var emailExists = await _userManager.Users
                .AnyAsync(x => x.Email == request.Email, cancellationToken);

            if (emailExists)
            {
                return Result.Failure(UserErrors.DublicatedEmail);
            }

            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
                EmailConfirmed = true
            };

            var createUserResult = await _userManager.CreateAsync(user, request.Password);

            if (!createUserResult.Succeeded)
            {
                var error = createUserResult.Errors.First();
                return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
            }

            var roleResult = await _userManager.AddToRoleAsync(user, DefaultRoles.SuperAdmin);

            if (!roleResult.Succeeded)
            {
                await _userManager.DeleteAsync(user);

                var error = roleResult.Errors.First();
                return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
            }

            return Result.Success();
        }
    }
}