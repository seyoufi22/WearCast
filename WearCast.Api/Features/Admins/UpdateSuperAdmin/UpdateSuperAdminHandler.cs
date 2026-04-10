

namespace WearCast.Api.Features.Admins.UpdateSuperAdmin
{
    public class UpdateSuperAdminHandler(
        ApplicationDbContext context,
        IHttpContextAccessor httpContextAccessor
    ) : IRequestHandler<UpdateSuperAdminRequest, Result>
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<Result> Handle(UpdateSuperAdminRequest request, CancellationToken cancellationToken)
        {
            var currentUserId = _httpContextAccessor.HttpContext!.User.GetUserId();

            if (string.IsNullOrEmpty(currentUserId))
            {
                return Result.Failure(new Error("User.InvalidToken", "SuperAdmin ID not found in token.", StatusCodes.Status401Unauthorized));
            }

            var superAdminUser = await _context.Users
                .FirstOrDefaultAsync(x => x.Id == currentUserId, cancellationToken);

            if (superAdminUser == null)
            {
                return Result.Failure(new Error("Admin.NotFound", "SuperAdmin user not found.", StatusCodes.Status404NotFound));
            }

            superAdminUser.FirstName = request.FirstName;
            superAdminUser.LastName = request.LastName;
            superAdminUser.PhoneNumber = request.PhoneNumber;

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}