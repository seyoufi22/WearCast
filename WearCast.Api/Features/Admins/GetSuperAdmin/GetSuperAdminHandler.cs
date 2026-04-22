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

            var response = await _context.Users
                .AsNoTracking()
                .Where(x => x.Id == currentUserId)
                .Select(x => new GetSuperAdminResponse(
                    x.Id,
                    x.FirstName,
                    x.LastName,
                    x.PhoneNumber
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