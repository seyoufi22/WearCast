namespace WearCast.Api.Features.DesignedProductManagement.FactoryProductColors.DeleteFactoryProductColor
{
    public class DeleteFactoryProductColorHandler(
          ApplicationDbContext context
        ) : IRequestHandler<DeleteFactoryProductColorRequest, Result>
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<Result> Handle(DeleteFactoryProductColorRequest request, CancellationToken cancellationToken)
        {
            var color = await _context.DesignedProductColors
               .FirstOrDefaultAsync(x =>
                   x.Slug == request.CurrentColorSlug &&
                   x.DesignedProduct.Slug == request.ProductSlug &&
                   !x.IsDeleted &&
                   !x.DesignedProduct.IsDeleted,
               cancellationToken);

            if (color == null)
            {
                return Result.Failure(FactoryProductColorErrors.ColorNotFound);
            }

            color.IsDeleted = true;

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
