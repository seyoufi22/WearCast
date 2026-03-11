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
                .FirstOrDefaultAsync(x => x.Slug == request.CurrentSlug, cancellationToken);

            if (product == null)
                return Result.Failure(DesignedProductErrors.ProductNotFound);

            if (user.IsSuperAdmin())
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

            if (product.Name != request.Name)
            {
                product.Slug = request.Name.ToUniqueSlug();
            }

            _mapper.Map(request, product);

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
