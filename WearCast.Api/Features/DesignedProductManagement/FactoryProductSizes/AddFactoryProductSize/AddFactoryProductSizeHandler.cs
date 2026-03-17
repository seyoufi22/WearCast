namespace WearCast.Api.Features.DesignedProductManagement.FactoryProductSizes.AddFactoryProductSize
{
    public class AddFactoryProductSizeHandler(
        ApplicationDbContext context,
        IHttpContextAccessor httpContextAccessor
        ) : IRequestHandler<AddFactoryProductSizeRequest, Result>
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<Result> Handle(AddFactoryProductSizeRequest request, CancellationToken cancellationToken)
        {
            var user = _httpContextAccessor.HttpContext!.User;


            var isSuperAdmin = user.IsSuperAdmin();

            var userFactoryId = user.GetFactoryId();

            if (!isSuperAdmin && userFactoryId == null)
            {
                return Result.Failure(AuthErrors.Forbidden);
            }

            var productData = await _context.DesignedProducts
                 .Where(x => x.Id == request.ProductId)
                 .Select(x => new
                 {
                     x.FactoryId,
                     HasSizeAlready = x.SizeDetails.Any(s => s.Size == request.Size)
                 })
                 .FirstOrDefaultAsync(cancellationToken);

            if (productData == null)
            {
                return Result.Failure(DesignedProductErrors.ProductNotFound);
            }

            if (!isSuperAdmin && (userFactoryId == null || productData.FactoryId != userFactoryId.Value))
            {
                return Result.Failure(AuthErrors.Forbidden);
            }

            if (productData.HasSizeAlready)
            {
                return Result.Failure(SizeErrors.SizeAlreadyExists);
            }

            var newSize = new DesignedProductSizeDetails
            {
                DesignedProductId = request.ProductId,
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