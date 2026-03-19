namespace WearCast.Api.Features.DesignedProductManagement.FactoryProducts.CreateDesignedProduct
{
    public class CreateDesignedProductHandler(
        ApplicationDbContext context,
        IMapper mapper,
        IHttpContextAccessor httpContextAccessor
        ) : IRequestHandler<CreateDesignedProductRequest, Result<CreateDesignedProductResponse>>
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IMapper _mapper = mapper;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<Result<CreateDesignedProductResponse>> Handle(CreateDesignedProductRequest request, CancellationToken cancellationToken)
        {
            var user = _httpContextAccessor.HttpContext!.User;

            int finalFactoryId;

            if (user.IsSuperAdmin())
            {
                if (!request.FactoryId.HasValue || request.FactoryId <= 0)
                {
                    return Result.Failure<CreateDesignedProductResponse>(DesignedProductErrors.FactoryRequiredForAdmin);
                }

                var factoryExists = await _context.Factories.AnyAsync(f => f.Id == request.FactoryId.Value, cancellationToken);
                if (!factoryExists)
                {
                    return Result.Failure<CreateDesignedProductResponse>(DesignedProductErrors.FactoryNotFound);
                }

                finalFactoryId = request.FactoryId.Value;
            }
            else if (user.IsFactoryManager())
            {
                var factoryIdFromToken = user.GetFactoryId();


                if (factoryIdFromToken == null)
                {
                    return Result.Failure<CreateDesignedProductResponse>(AuthErrors.NoAssociatedFactory);
                }

                finalFactoryId = factoryIdFromToken.Value;
            }
            else
            {
                return Result.Failure<CreateDesignedProductResponse>(AuthErrors.Forbidden);
            }

            var product = _mapper.Map<DesignedProduct>(request);

            product.TargetAudience = request.TargetAudiences.Aggregate((current, next) => current | next);

            product.FactoryId = finalFactoryId;

            _context.DesignedProducts.Add(product);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success(new CreateDesignedProductResponse(product.Id));
        }
    }
}