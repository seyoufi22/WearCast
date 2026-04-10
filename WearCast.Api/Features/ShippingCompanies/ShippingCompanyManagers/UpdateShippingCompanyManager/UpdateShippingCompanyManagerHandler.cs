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
                    return Result.Failure(new Error("Validation.MissingId", "SuperAdmin must provide a target ProviderId to delete.", StatusCodes.Status400BadRequest));
                }

                targetManagerId = request.ProvidedManagerId.Value;
            }
            else
            {
                targetManagerId = user.GetShippingCompanyManagerId()!.Value;
            }

            var managerUser = await _context.Users
                .FirstOrDefaultAsync(x => x.ShippingCompanyManager.Id == targetManagerId, cancellationToken);

            if (managerUser == null)
            {
                return Result.Failure(ShippingCompanyManagerErrors.NotFound);
            }

            managerUser.FirstName = request.FirstName;
            managerUser.LastName = request.LastName;
            managerUser.PhoneNumber = request.PhoneNumber;

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
