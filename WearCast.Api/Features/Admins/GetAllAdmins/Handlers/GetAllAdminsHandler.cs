using WearCast.Api.Common.Helper;
using WearCast.Api.Common.Views;
using WearCast.Api.Features.Admins.GetAllAdmins.DTOs;

namespace WearCast.Api.Features.Admins.GetAllAdmins.Handlers
{
    public class GetAllAdminsHandler : IRequestHandler<GetAllAdminsRequestDTO, Result<PagingViewModel<GetAllAdminsResponseDTO>>>
    {
        private readonly ApplicationDbContext _context;
        private static readonly string[] AdminRoleNames = Enum.GetNames(typeof(AdminRole));
        public GetAllAdminsHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<PagingViewModel<GetAllAdminsResponseDTO>>> Handle(
            GetAllAdminsRequestDTO request,
            CancellationToken cancellationToken)
        {
            var baseQuery =
                from user in _context.Users.AsNoTracking()
                join ur in _context.UserRoles on user.Id equals ur.UserId
                join r in _context.Roles on ur.RoleId equals r.Id
                where AdminRoleNames.Contains(r.Name)
                select new
                {
                    User = user,
                    RoleName = r.Name
                };

            if (!string.IsNullOrWhiteSpace(request.FirstName))
            {
                baseQuery = baseQuery.Where(u => u.User.FirstName.Contains(request.FirstName.Trim()));
            }
            if (!string.IsNullOrWhiteSpace(request.LastName))
            {
                baseQuery = baseQuery.Where(u => u.User.LastName.Contains(request.LastName.Trim()));
            }
            if (!string.IsNullOrWhiteSpace(request.Email))
            {
                baseQuery = baseQuery.Where(u => u.User.Email.Contains(request.Email.Trim()));
            }
            if (request.Role.HasValue)
            {
                var requestedRoleName = request.Role.Value.ToString();
                baseQuery = baseQuery.Where(u => u.RoleName == requestedRoleName);
            }

            baseQuery = baseQuery.OrderBy(u => u.User.FirstName).ThenBy(u => u.User.LastName);

            var finalQuery = baseQuery.Select(u => new GetAllAdminsResponseDTO
            {
                Id = u.User.Id,
                FullName = u.User.FirstName + " " + u.User.LastName,
                Email = u.User.Email,
                PhoneNumber = u.User.PhoneNumber,
                Role = u.RoleName ?? "No role"
            });

            var pagedResult = await PagingHelper.CreateAsync(finalQuery, request.PageIndex, request.PageSize);

            return Result.Success(pagedResult);
        }
    }
}
