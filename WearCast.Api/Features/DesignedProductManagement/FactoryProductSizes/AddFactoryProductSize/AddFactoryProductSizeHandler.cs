namespace WearCast.Api.Features.DesignedProductManagement.FactoryProductSizes.AddFactoryProductSize
{
    public class AddFactoryProductSizeHandler(
        ApplicationDbContext context
        ) : IRequestHandler<AddFactoryProductSizeRequest, Result>
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<Result> Handle(AddFactoryProductSizeRequest request, CancellationToken cancellationToken)
        {
            var productData = await _context.DesignedProducts
                 .Where(x => x.Slug == request.ProductSlug)
                 .Select(x => new
                 {
                     ProductId = x.Id,
                     HasSizeAlready = x.SizeDetails.Any(s => s.Size == request.Size)
                 })
                 .FirstOrDefaultAsync(cancellationToken);

            if (productData == null)
            {
                return Result.Failure(DesignedProductErrors.ProductNotFound);
            }

            if (productData.HasSizeAlready)
            {
                return Result.Failure(SizeErrors.SizeAlreadyExists);
            }

            var newSize = new DesignedProductSizeDetails
            {
                DesignedProductId = productData.ProductId,
                Size = request.Size,
                A = request.A,
                B = request.B,
                C = request.C
            };

            _context.DesignedProductSizeDetails.Add(newSize);

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
