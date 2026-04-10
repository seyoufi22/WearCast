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
                targetManagerId = user.GetFactoryManagerId()!.Value;
            }
            var managerUser = await _context.Users
                .FirstOrDefaultAsync(u => u.FactoryManager.Id == targetManagerId, cancellationToken);

            if (managerUser == null)
            {
                return Result.Failure(FactoryManagerErrors.NotFound);
            }

            managerUser.FirstName = request.FirstName;
            managerUser.LastName = request.LastName;
            managerUser.PhoneNumber = request.PhoneNumber;

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
