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

            var product = await _context.DesignedProducts
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (product == null)
            {
                return Result.Failure(DesignedProductErrors.ProductNotFound);
            }

            if (user.IsSuperAdmin() || user.IsCatalogAdmin())
            {
                // No Action needed
            }
            else if (user.IsFactoryManager())
            {
                var factoryIdFromToken = user.GetFactoryId();
                if (factoryIdFromToken == null)
                    return Result.Failure(AuthErrors.NoAssociatedFactory);

                if (product.FactoryId != factoryIdFromToken.Value)
                    return Result.Failure(AuthErrors.Forbidden);
            }
            else
            {
                return Result.Failure(AuthErrors.Forbidden);
            }


            await _context.DesignedProductSizeDetails
                .Where(s => s.DesignedProductId == request.Id)
                .ExecuteUpdateAsync(s => s.SetProperty(x => x.IsDeleted, true), cancellationToken);

            await _context.DesignedProductReviews
                .Where(r => r.DesignedProductId == request.Id)
                .ExecuteUpdateAsync(r => r.SetProperty(x => x.IsDeleted, true), cancellationToken);

            await _context.DesignedProductImages
                .Where(img => _context.DesignedProductColors
                    .Any(color => color.DesignedProductId == request.Id && color.Id == img.DesignedProductColorId))
                .ExecuteUpdateAsync(i => i.SetProperty(x => x.IsDeleted, true), cancellationToken);

            await _context.DesignedProductColors
                .Where(color => color.DesignedProductId == request.Id)
                .ExecuteUpdateAsync(setters => setters.SetProperty(c => c.IsDeleted, true), cancellationToken);

            await _context.CustomerDesigns
                .Where(cd => cd.DesignedProductId == request.Id)
                .ExecuteUpdateAsync(cd => cd.SetProperty(x => x.IsDeleted, true), cancellationToken);


            // 4. Soft delete the main product itself
            product.IsDeleted = true;
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}