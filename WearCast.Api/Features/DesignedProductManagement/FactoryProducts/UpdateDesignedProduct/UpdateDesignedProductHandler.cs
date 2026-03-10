namespace WearCast.Api.Features.DesignedProductManagement.FactoryProducts.UpdateDesignedProduct
{
    public class UpdateDesignedProductHandler(
        ApplicationDbContext context,
        IMapper mapper
        ) : IRequestHandler<UpdateDesignedProductRequest, Result>
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<Result> Handle(UpdateDesignedProductRequest request, CancellationToken cancellationToken)
        {
            var product = await _context.DesignedProducts
                .FirstOrDefaultAsync(x => x.Slug == request.CurrentSlug, cancellationToken);

            if (product == null)
                return Result.Failure(DesignedProductErrors.ProductNotFound);

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
