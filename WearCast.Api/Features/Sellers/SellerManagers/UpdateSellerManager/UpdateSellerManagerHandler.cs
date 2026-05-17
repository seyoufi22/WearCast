using WearCast.Api.Features.AuthenticationManagement;

namespace WearCast.Api.Features.Sellers.SellerManagers.UpdateSellerManager
{
    public class UpdateSellerManagerHandler(
        ApplicationDbContext context,
        IHttpContextAccessor httpContextAccessor
        ) : IRequestHandler<UpdateSellerManagerRequest, Result>
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        public async Task<Result> Handle(UpdateSellerManagerRequest request, CancellationToken cancellationToken)
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
                targetManagerId = user.GetSellerManagerId()!.Value;
            }

            var managerUser = await _context.Users
                .FirstOrDefaultAsync(x => x.SellerManager.Id == targetManagerId, cancellationToken);

            if (managerUser == null)
            {
                return Result.Failure(SellerManagerErrors.NotFound);
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
