namespace WearCast.Api.Features.DesignedProductManagement.FactoryProductSizes.UpdateFactoryProductSize
{
    public class UpdateFactoryProductSizeHandler(
        ApplicationDbContext context,
        IHttpContextAccessor httpContextAccessor
        ) : IRequestHandler<UpdateFactoryProductSizeRequest, Result>
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<Result> Handle(UpdateFactoryProductSizeRequest request, CancellationToken cancellationToken)
        {
            var user = _httpContextAccessor.HttpContext!.User;

            var isSuperAdmin = user.IsSuperAdmin();
            var userFactoryId = user.GetFactoryId();

            var data = await _context.DesignedProductSizeDetails
                .Where(x => x.Id == request.Id)
                .Select(x => new
                {
                    SizeEntity = x,
                    FactoryId = x.DesignedProduct.FactoryId
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (data == null)
            {
                return Result.Failure(SizeErrors.SizeNotFound);
            }

            if (!isSuperAdmin && (userFactoryId == null || data.FactoryId != userFactoryId.Value))
            {
                return Result.Failure(AuthErrors.Forbidden);
            }

            data.SizeEntity.A = request.A;
            data.SizeEntity.B = request.B;
            data.SizeEntity.C = request.C;

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}