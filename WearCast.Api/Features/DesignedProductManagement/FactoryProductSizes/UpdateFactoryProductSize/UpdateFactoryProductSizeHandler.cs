namespace WearCast.Api.Features.DesignedProductManagement.FactoryProductSizes.UpdateFactoryProductSize
{
    public class UpdateFactoryProductSizeHandler(
        ApplicationDbContext context
        ) : IRequestHandler<UpdateFactoryProductSizeRequest, Result>
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<Result> Handle(UpdateFactoryProductSizeRequest request, CancellationToken cancellationToken)
        {
            var sizeDetails = await _context.DesignedProductSizeDetails
                .FirstOrDefaultAsync(x =>
                    x.Size == request.Size &&
                    x.DesignedProduct.Slug == request.ProductSlug,
                cancellationToken);

            if (sizeDetails == null)
                return Result.Failure(SizeErrors.SizeNotFound);

            sizeDetails.A = request.A;
            sizeDetails.B = request.B;
            sizeDetails.C = request.C;

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
