namespace WearCast.Api.Features.DesignedProductManagement.FactoryProductColors.UpdateFactoryProductColor
{
    public class UpdateFactoryProductColorHandler(
        ApplicationDbContext context
        ) : IRequestHandler<UpdateFactoryProductColorRequest, Result>
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<Result> Handle(UpdateFactoryProductColorRequest request, CancellationToken cancellationToken)
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

            if (color.Name != request.Name)
            {
                color.Name = request.Name;
                color.Slug = request.Name.ToUniqueSlug();
            }

            color.HexCode = request.HexCode;

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();

        }
    }
}
