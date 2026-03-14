namespace WearCast.Api.Features.DesignedProductManagement.FactoryProductSizes.DeleteFactoryProductSize
{
    public class DeleteFactoryProductSizeHandler(ApplicationDbContext context) : IRequestHandler<DeleteFactoryProductSizeRequest, Result>
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<Result> Handle(DeleteFactoryProductSizeRequest request, CancellationToken cancellationToken)
        {
            var sizeDetails = await _context.DesignedProductSizeDetails
                .FirstOrDefaultAsync(x =>
                x.Size == request.Size &&
                x.DesignedProduct.Slug == request.ProductSlug,
                cancellationToken);

            if (sizeDetails == null)
                return Result.Failure(SizeErrors.SizeNotFound);

            sizeDetails.IsDeleted = true;

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
