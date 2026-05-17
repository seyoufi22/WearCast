namespace WearCast.Api.Features.DesignedProductManagement.FactoryProducts.UpdateDesignedProduct
{
    public class UpdateDesignedProductHandler(
        ApplicationDbContext context,
        IMapper mapper,
        IHttpContextAccessor httpContextAccessor
        ) : IRequestHandler<UpdateDesignedProductRequest, Result>
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IMapper _mapper = mapper;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<Result> Handle(UpdateDesignedProductRequest request, CancellationToken cancellationToken)
        {
            var user = _httpContextAccessor.HttpContext!.User;

            var product = await _context.DesignedProducts
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (product == null)
                return Result.Failure(DesignedProductErrors.ProductNotFound);

            if (user.IsSuperAdmin() || user.IsCatalogAdmin())
            {
                //No Action
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

            var categoryExists = await _context.Categories
                .AnyAsync(c => c.Id == request.CategoryId, cancellationToken);

            if (!categoryExists)
            {
                return Result.Failure(new("Category.NotFound", "The specified category was not found.", StatusCodes.Status404NotFound));
            }

            if (request.DefaultColorId.HasValue)
            {
                var isValidColor = await _context.DesignedProductColors
                    .AnyAsync(c => c.Id == request.DefaultColorId.Value
                                && c.DesignedProductId == product.Id
                                , cancellationToken);

                if (!isValidColor)
                {
                    return Result.Failure(new Error(
                        "ProductColor.Invalid",
                        "The selected default color does not exist or does not belong to this product.",
                        StatusCodes.Status400BadRequest));
                }
            }


            _mapper.Map(request, product);

            product.TargetAudience = request.TargetAudiences.Aggregate((current, next) => current | next);

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
