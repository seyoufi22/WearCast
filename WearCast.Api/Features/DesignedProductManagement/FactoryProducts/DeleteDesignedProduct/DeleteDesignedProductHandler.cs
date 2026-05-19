namespace WearCast.Api.Features.DesignedProductManagement.FactoryProducts.DeleteDesignedProduct
{
    public class DeleteDesignedProductHandler(
        ApplicationDbContext context,
        IHttpContextAccessor httpContextAccessor
        ) : IRequestHandler<DeleteDesignedProductRequest, Result>
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<Result> Handle(DeleteDesignedProductRequest request, CancellationToken cancellationToken)
        {
            var user = _httpContextAccessor.HttpContext!.User;

            // 1. Fetch the main designed product
            var product = await _context.DesignedProducts
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (product == null)
            {
                return Result.Failure(DesignedProductErrors.ProductNotFound);
            }

            // 2. Check Permissions
            if (user.IsSuperAdmin() || user.IsCatalogAdmin())
            {
                // No Action needed, they have full access
            }
            else if (user.IsFactoryManager())
            {
                var factoryIdFromToken = user.GetFactoryId();

                if (factoryIdFromToken == null)
                {
                    return Result.Failure(AuthErrors.NoAssociatedFactory);
                }

                if (product.FactoryId != factoryIdFromToken.Value)
                {
                    return Result.Failure(AuthErrors.Forbidden);
                }
            }
            else
            {
                return Result.Failure(AuthErrors.Forbidden);
            }

            // 3. Soft delete all related Designed Product Colors in one fast DB query
            // Note: Ensure your DbSet is named 'DesignedProductColors' and the foreign key is 'DesignedProductId'
            await _context.DesignedProductColors
                .Where(color => color.DesignedProductId == request.Id)
                .ExecuteUpdateAsync(setters => setters.SetProperty(c => c.IsDeleted, true), cancellationToken);

            // 4. Soft delete the main product itself
            product.IsDeleted = true;
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}