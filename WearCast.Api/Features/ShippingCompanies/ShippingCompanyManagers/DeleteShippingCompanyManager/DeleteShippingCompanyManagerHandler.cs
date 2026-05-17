using WearCast.Api.Features.Admins.DeleteAdmin.DTOs;
using WearCast.Api.Features.Admins.DeleteAdmin.Handlers;

namespace WearCast.Api.Features.ShippingCompanies.ShippingCompanyManagers.DeleteShippingCompanyManager
{
    public class DeleteShippingCompanyManagerHandler : IRequestHandler<DeleteShippingCompanyManagerRequest, Result>
    {
        private readonly ApplicationDbContext _context;
        public DeleteShippingCompanyManagerHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(DeleteShippingCompanyManagerRequest request, CancellationToken cancellationToken)
        {
            var manager = await _context.ShippingCompanyManagers
                .Include(m => m.ApplicationUser)
                .FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken);

            if (manager == null)
            {
                return Result.Failure(ShippingCompanyManagerErrors.NotFound);
            }

            if (manager.IsDeleted)
            {
                return Result.Failure(ShippingCompanyManagerErrors.AlreadyDeleted);
            }

            manager.IsDeleted = true;

            if (manager.ApplicationUser != null)
            {
                manager.ApplicationUser.IsDeleted = true;
            }

            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
