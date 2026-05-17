using WearCast.Api.Features.Admins.DeleteAdmin.DTOs;

namespace WearCast.Api.Features.Admins.DeleteAdmin.Handlers
{
    public class DeleteAdminHandler : IRequestHandler<DeleteAdminRequestDTO, Result>
    {
        private readonly ApplicationDbContext _context;
        private readonly EmailHelper _emailHelper;
        private readonly ILogger<DeleteAdminHandler> _logger;

        private static readonly string[] AdminRoleNames = Enum.GetNames(typeof(AdminRole));

        public DeleteAdminHandler(
            ApplicationDbContext context,
            EmailHelper emailHelper,
            ILogger<DeleteAdminHandler> logger)
        {
            _context = context;
            _emailHelper = emailHelper;
            _logger = logger;
        }

        public async Task<Result> Handle(
            DeleteAdminRequestDTO request,
            CancellationToken cancellationToken)
        {
            var adminData = await (
                from u in _context.Users.IgnoreQueryFilters()
                join ur in _context.UserRoles on u.Id equals ur.UserId
                join r in _context.Roles on ur.RoleId equals r.Id
                where u.Id == request.Id
                select new
                {
                    u.Id,
                    u.IsDeleted,
                    u.Email,          
                    u.FirstName,    
                    u.LastName,       
                    RoleName = r.Name
                }
            ).FirstOrDefaultAsync(cancellationToken);

            if (adminData == null)
                return Result.Failure(AdminErrors.NotFound);

            if (!AdminRoleNames.Contains(adminData.RoleName))
                return Result.Failure(AdminErrors.NotAnAdmin);

            if (adminData.RoleName == DefaultRoles.SuperAdmin)
                return Result.Failure(AdminErrors.CannotDeleteSuperAdmin);

            if (adminData.IsDeleted)
                return Result.Failure(AdminErrors.AlreadyDeleted);

            await _context.Users
                .IgnoreQueryFilters()
                .Where(u => u.Id == request.Id)
                .ExecuteUpdateAsync(setters => setters.SetProperty(u => u.IsDeleted, true), cancellationToken);

            try
            {
                await _emailHelper.SendAccountDeletedEmail(
                    adminData.Email!,
                    $"{adminData.FirstName} {adminData.LastName}",
                    "" 
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send deletion email to Admin: {Email} for AdminId: {AdminId}", adminData.Email, request.Id);
            }

            return Result.Success();
        }
    }
}