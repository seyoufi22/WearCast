namespace WearCast.Api.Features.DesignedProductManagement.FactoryProducts.DeleteDesignedProduct
{
    public class DeleteDesignedProductHandler(
        ApplicationDbContext context
        ) : IRequestHandler<DeleteDesignedProductRequest, Result>
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<Result> Handle(DeleteDesignedProductRequest request, CancellationToken cancellationToken)
        {
            var product = await _context.DesignedProducts.FirstOrDefaultAsync(x => x.Slug == request.Slug, cancellationToken);

            if (product == null)
            {
                return Result.Failure(DesignedProductErrors.ProductNotFound);
            }

            product.IsDeleted = true;

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
