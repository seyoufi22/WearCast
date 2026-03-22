namespace WearCast.Api.Features.DesignedProductManagement.CustomerCatalogAndWorkspace.GetProductDetails
{
    public class GetProductDetailsHandler(ApplicationDbContext context) : IRequestHandler<GetProductDetailsRequest, Result<GetProductDetailsResponse>>
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<Result<GetProductDetailsResponse>> Handle(GetProductDetailsRequest request, CancellationToken cancellationToken)
        {
            var product = await _context.DesignedProducts
                .Include(p => p.Colors) // فلترة الـ Soft Delete جوه الـ Include (أدائها أعلى في EF Core)
                    .ThenInclude(c => c.Images)
                .Include(p => p.SizeDetails)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (product == null)
            {
                return Result.Failure<GetProductDetailsResponse>(
                   new Error(
                       "Catalog.ProductNotFound",
                       "The requested product was not found or is no longer available.",
                       StatusCodes.Status404NotFound
                   )
               );
            }

            var response = new GetProductDetailsResponse(
                product.Id,
                product.Name,
                product.Description,
                product.TargetAudience.ToString().Split(", ").ToList(),
                product.Price,
                product.CanvasWidth,
                product.CanvasHeight,

                product.SizeDetails.Select(s => new SizeDetailsResponse(
                    s.Size.ToString(),
                    s.A,
                    s.B,
                    s.C
                )).ToList(),

                product.Colors.Select(c => new ColorVariantResponse(
                    c.Id,
                    c.Name,
                    c.HexCode,
                    c.Images.Select(img => new ImageResponse(
                        img.ImageUrl,
                        img.ViewSide.ToString()
                    )).ToList()
                )).ToList()
            );

            return Result.Success(response);
        }
    }
}
