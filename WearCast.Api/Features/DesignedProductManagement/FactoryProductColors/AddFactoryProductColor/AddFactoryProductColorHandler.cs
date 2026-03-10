namespace WearCast.Api.Features.DesignedProductManagement.FactoryProductColors.AddFactoryProductColor
{
    public class AddFactoryProductColorHandler(
        ApplicationDbContext context
        ) : IRequestHandler<AddFactoryProductColorRequest, Result<AddFactoryProductColorResponse>>
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<Result<AddFactoryProductColorResponse>> Handle(AddFactoryProductColorRequest request, CancellationToken cancellationToken)
        {
            var productId = await _context.DesignedProducts
                 .Where(x => x.Slug == request.ProductSlug && !x.IsDeleted)
                 .Select(x => (int?)x.Id)
                 .FirstOrDefaultAsync(cancellationToken);

            if (productId == null)
            {
                return Result.Failure<AddFactoryProductColorResponse>(DesignedProductErrors.ProductNotFound);
            }

            var colorSlug = request.Name.ToUniqueSlug();

            var newProductColor = new DesignedProductColor
            {
                Name = request.Name,
                HexCode = request.HexCode,
                Slug = colorSlug,
                DesignedProductId = productId.Value
            };

            _context.DesignedProductColors.Add(newProductColor);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success(new AddFactoryProductColorResponse(colorSlug));
        }
    }
}
