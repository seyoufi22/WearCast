namespace WearCast.Api.Features.DesignedProductManagement.FactoryProducts.CreateDesignedProduct
{
    public class CreateDesignedProductHandler(
        ApplicationDbContext context,
        IMapper mapper
        ) : IRequestHandler<CreateDesignedProductRequest, Result<CreateDesignedProductResponse>>
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<Result<CreateDesignedProductResponse>> Handle(CreateDesignedProductRequest request, CancellationToken cancellationToken)
        {
            var product = _mapper.Map<DesignedProduct>(request);

            _context.DesignedProducts.Add(product);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success(new CreateDesignedProductResponse(product.Slug));
        }
    }
}
