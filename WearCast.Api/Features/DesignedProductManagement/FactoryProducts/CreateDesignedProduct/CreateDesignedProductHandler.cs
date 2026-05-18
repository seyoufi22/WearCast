using WearCast.Api.Features.FixedProduct.CreateFixedProduct.Notifications;

namespace WearCast.Api.Features.DesignedProductManagement.FactoryProducts.CreateDesignedProduct
{
    public class CreateDesignedProductHandler(
        ApplicationDbContext context,
        IMapper mapper,
        IHttpContextAccessor httpContextAccessor,
        IMediator mediator
        ) : IRequestHandler<CreateDesignedProductRequest, Result<CreateDesignedProductResponse>>
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IMapper _mapper = mapper;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly IMediator _mediator = mediator;

        public async Task<Result<CreateDesignedProductResponse>> Handle(CreateDesignedProductRequest request, CancellationToken cancellationToken)
        {
            var user = _httpContextAccessor.HttpContext!.User;

            int finalFactoryId;

            if (user.IsSuperAdmin() || user.IsCatalogAdmin())
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

            var categoryExists = await _context.Categories.AnyAsync(c => c.Id == request.CategoryId, cancellationToken);
            if (!categoryExists)
            {
                return Result.Failure<CreateDesignedProductResponse>(new Error("Category.NotFound", "The specified category does not exist.", StatusCodes.Status404NotFound));
            }

            var product = _mapper.Map<DesignedProduct>(request);

            product.TargetAudience = request.TargetAudiences.Aggregate((current, next) => current | next);

            product.FactoryId = finalFactoryId;

            _context.DesignedProducts.Add(product);
            await _context.SaveChangesAsync(cancellationToken);


            var catalogAdminUserIds = await (from adminUser in _context.Users
                                             join ur in _context.UserRoles on adminUser.Id equals ur.UserId
                                             join r in _context.Roles on ur.RoleId equals r.Id
                                             where r.Name == DefaultRoles.CatalogAdmin && !adminUser.IsDeleted
                                             select adminUser.Id).ToListAsync(cancellationToken);
            if (catalogAdminUserIds.Any())
            {
                var notificationEvent = new NewProductEvent(
                    RecipientIds: catalogAdminUserIds,
                    ProductId: product.Id,
                    ProductName: product.Name,
                    ProductType: "Designed Product");
                await _mediator.Publish(notificationEvent, cancellationToken);
            }

            return Result.Success(new CreateDesignedProductResponse(product.Id));
        }
    }
}