using MediatR;
using Microsoft.EntityFrameworkCore;

namespace WearCast.Api.Features.Admins.GetSuperAdmin
{
    public class GetSuperAdminHandler(
        ApplicationDbContext context,
        IHttpContextAccessor httpContextAccessor
    ) : IRequestHandler<GetSuperAdminRequest, Result<GetSuperAdminResponse>>
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<Result<GetSuperAdminResponse>> Handle(GetSuperAdminRequest request, CancellationToken cancellationToken)
        {

            var currentUserId = _httpContextAccessor.HttpContext!.User.GetUserId();

            if (string.IsNullOrEmpty(currentUserId))
            {
                return Result.Failure<GetSuperAdminResponse>(new Error("User.InvalidToken", "SuperAdmin ID not found in token.", StatusCodes.Status401Unauthorized));
            }
            var baseQuery =
                from user in _context.Users.AsNoTracking()
                join ur in _context.UserRoles on user.Id equals ur.UserId
                join r in _context.Roles on ur.RoleId equals r.Id
                select new
                {
                    User = user,
                    RoleName = r.Name
                };
            var response = await baseQuery
                .Where(x => x.User.Id == currentUserId)
                .Select(x => new GetSuperAdminResponse(
                    x.User.Id,
                    x.User.FirstName,
                    x.User.LastName,
                    x.User.Email ?? string.Empty,
                    x.RoleName,
                    x.User.PhoneNumber
                ))
                .FirstOrDefaultAsync(cancellationToken);

            if (response == null)
            {
                return Result.Failure<GetSuperAdminResponse>(new Error("Admin.NotFound", "SuperAdmin user not found.", StatusCodes.Status404NotFound));
            }

            return Result.Success(response);
        }
    }
}