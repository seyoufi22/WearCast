using WearCast.Api.Features.Admins.GetAdminById.DTOs;

namespace WearCast.Api.Features.Admins.GetAdminById.Handlers
{
    public class GetAdminByIdHandler : IRequestHandler<GetAdminByIdRequestDTO, Result<GetAdminByIdResponseDTO>>
    {
        private readonly ApplicationDbContext _context;
        private static readonly string[] AdminRoleNames = Enum.GetNames(typeof(AdminRole));

        public GetAdminByIdHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<GetAdminByIdResponseDTO>> Handle(GetAdminByIdRequestDTO request, CancellationToken cancellationToken)
        {
            var adminData = await (from user in _context.Users.AsNoTracking()
                                   join ur in _context.UserRoles on user.Id equals ur.UserId
                                   join r in _context.Roles on ur.RoleId equals r.Id
                                   where user.Id == request.Id && AdminRoleNames.Contains(r.Name)
                                   select new GetAdminByIdResponseDTO
                                   {
                                       Id = user.Id,
                                       FullName = user.FirstName + " " + user.LastName,
                                       Email = user.Email,
                                       PhoneNumber = user.PhoneNumber,
                                       Role = r.Name ?? "No role"
                                   }).FirstOrDefaultAsync(cancellationToken);

            if (adminData == null)
            {
                return Result.Failure<GetAdminByIdResponseDTO>(new Error("Admin.NotFound", "Admin not found.",404));
            }

            return Result.Success(adminData);
        }
    }
}