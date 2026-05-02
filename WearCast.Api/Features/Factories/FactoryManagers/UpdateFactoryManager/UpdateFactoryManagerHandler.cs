using WearCast.Api.Features.AuthenticationManagement;

namespace WearCast.Api.Features.Factories.FactoryManagers.UpdateFactoryManager
{
    public class UpdateFactoryManagerHandler(
        ApplicationDbContext context,
        IHttpContextAccessor httpContextAccessor
        ) : IRequestHandler<UpdateFactoryManagerRequest, Result>
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        public async Task<Result> Handle(UpdateFactoryManagerRequest request, CancellationToken cancellationToken)
        {
            var user = _httpContextAccessor.HttpContext!.User;

            int targetManagerId;

            if (user.IsSuperAdmin() || user.IsVendorAdmin())
            {
                if (!request.ProvidedManagerId.HasValue)
                {
                    return Result.Failure(new Error("Validation.MissingId", "SuperAdmin must provide a target ProviderId to delete.", StatusCodes.Status400BadRequest));
                }

                targetManagerId = request.ProvidedManagerId.Value;
            }
            else
            {
                targetManagerId = user.GetFactoryManagerId()!.Value;
            }
            var managerUser = await _context.Users
                .FirstOrDefaultAsync(u => u.FactoryManager.Id == targetManagerId, cancellationToken);

            if (managerUser == null)
            {
                return Result.Failure(FactoryManagerErrors.NotFound);
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
