namespace WearCast.Api.Features.DesignedProductManagement.FactoryProductColors.DeleteFactoryProductColor
{
    public class DeleteFactoryProductColorHandler(
          ApplicationDbContext context,
          IHttpContextAccessor httpContextAccessor
        ) : IRequestHandler<DeleteFactoryProductColorRequest, Result>
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<Result> Handle(DeleteFactoryProductColorRequest request, CancellationToken cancellationToken)
        {
            var user = _httpContextAccessor.HttpContext!.User;

            var queryResult = await _context.DesignedProductColors
               .Where(x => x.Id == request.ColorId)
               .Select(x => new
               {
                   Color = x,
                   FactoryId = x.DesignedProduct.FactoryId
               })
               .FirstOrDefaultAsync(cancellationToken);

            if (queryResult == null)
            {
                return Result.Failure(FactoryProductColorErrors.ColorNotFound);
            }

            var color = queryResult.Color;
            var productFactoryId = queryResult.FactoryId;


            if (user.IsSuperAdmin() || user.IsCatalogAdmin())
            {
                // No Action
            }
            else if (user.IsFactoryManager())
            {
                var factoryIdFromToken = user.GetFactoryId();

                if (factoryIdFromToken == null)
                {
                    return Result.Failure(AuthErrors.NoAssociatedFactory);
                }

                if (productFactoryId != factoryIdFromToken.Value)
                {
                    return Result.Failure(AuthErrors.Forbidden);
                }
            }
            else
            {
                return Result.Failure(AuthErrors.Forbidden);
            }

            await _context.DesignedProductImages
                    .Where(img => img.DesignedProductColorId == request.ColorId)
                    .ExecuteUpdateAsync(img => img.SetProperty(x => x.IsDeleted, true), cancellationToken);

            await _context.CustomerDesigns
                .Where(cd => cd.DesignedProductColorId == request.ColorId)
                .ExecuteUpdateAsync(cd => cd.SetProperty(x => x.IsDeleted, true), cancellationToken);

            color.IsDeleted = true;

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
