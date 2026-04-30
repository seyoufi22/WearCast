using WearCast.Api.Features.Admins.DeleteAdmin.DTOs;

namespace WearCast.Api.Features.Admins.DeleteAdmin.Handlers
{
    public class DeleteAdminHandler : IRequestHandler<DeleteAdminRequestDTO, Result>
    {
        private readonly ApplicationDbContext _context;
        private static readonly string[] AdminRoleNames = Enum.GetNames(typeof(AdminRole));

        public DeleteAdminHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(
            DeleteAdminRequestDTO request,
            CancellationToken cancellationToken)
        {
            var result = await (
                from u in _context.Users.IgnoreQueryFilters()
                join ur in _context.UserRoles on u.Id equals ur.UserId
                join r in _context.Roles on ur.RoleId equals r.Id
                where u.Id == request.Id
                select new
                {
                    User = u,
                    RoleName = r.Name
                }
            ).FirstOrDefaultAsync(cancellationToken);

            if (result == null)
            {
                return Result.Failure(AdminErrors.NotFound);
            }

            var user = result.User;
            var roleName = result.RoleName;

            if (!AdminRoleNames.Contains(roleName))
            {
                return Result.Failure(AdminErrors.NotAnAdmin);
            }
            if (roleName == DefaultRoles.SuperAdmin)
            {
                return Result.Failure(AdminErrors.CannotDeleteSuperAdmin);
            }
            if (user.IsDeleted)
            {
                return Result.Failure(AdminErrors.AlreadyDeleted);
            }

            user.IsDeleted = true;
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
