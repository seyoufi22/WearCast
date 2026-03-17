namespace WearCast.Api.Features.DesignedProductManagement.FactoryProductSizes.DeleteFactoryProductSize
{
    public class DeleteFactoryProductSizeHandler(
        ApplicationDbContext context,
        IHttpContextAccessor httpContextAccessor
        ) : IRequestHandler<DeleteFactoryProductSizeRequest, Result>
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<Result> Handle(DeleteFactoryProductSizeRequest request, CancellationToken cancellationToken)
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

            data.SizeEntity.IsDeleted = true;

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
