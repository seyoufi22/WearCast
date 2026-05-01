using WearCast.Api.Common.Extensions;
using WearCast.Api.Features.AuthenticationManagement;

namespace WearCast.Api.Features.ShippingCompanies.ShippingCompanyManagers.UpdateShippingCompanyManager
{
    public class UpdateShippingCompanyManagerHandler(
         ApplicationDbContext context,
         IHttpContextAccessor httpContextAccessor
        ) : IRequestHandler<UpdateShippingCompanyManagerRequest, Result>
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<Result> Handle(UpdateShippingCompanyManagerRequest request, CancellationToken cancellationToken)
        {
            var user = _httpContextAccessor.HttpContext!.User;
            int targetManagerId;

            if (user.IsSuperAdmin())
            {
                if (!request.ProvidedManagerId.HasValue)
                {
                    return Result.Failure(new Error("Validation.MissingId", "SuperAdmin must provide a target ManagerId to update.", StatusCodes.Status400BadRequest));
                }

                targetManagerId = request.ProvidedManagerId.Value;
            }
            else
            {
                var managerId = user.GetShippingCompanyManagerId();
                if (!managerId.HasValue)
                {
                    return Result.Failure(ShippingCompanyManagerErrors.NotFound);
                }
                targetManagerId = managerId.Value;
            }

            var managerUser = await _context.Users
                .FirstOrDefaultAsync(x => x.ShippingCompanyManager != null && x.ShippingCompanyManager.Id == targetManagerId, cancellationToken);

            if (managerUser == null)
            {
                return Result.Failure(ShippingCompanyManagerErrors.NotFound);
            }

            bool isPhoneNumberTaken = await _context.Users
                .AnyAsync(x => x.PhoneNumber == request.PhoneNumber && x.Id != managerUser.Id, cancellationToken);

            if (isPhoneNumberTaken)
            {
                return Result.Failure(UserErrors.DublicatedPhoneNumber);
            }

            managerUser.FirstName = request.FirstName;
            managerUser.LastName = request.LastName;
            managerUser.PhoneNumber = request.PhoneNumber;

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}

