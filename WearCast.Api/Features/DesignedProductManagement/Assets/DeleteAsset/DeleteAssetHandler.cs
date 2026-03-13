namespace WearCast.Api.Features.DesignedProductManagement.Assets.DeleteAsset
{
    public class DeleteAssetHandler(ApplicationDbContext context) : IRequestHandler<DeleteAssetRequest, Result>
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<Result> Handle(DeleteAssetRequest request, CancellationToken cancellationToken)
        {
            var asset = await _context.DesignAssets
                .FirstOrDefaultAsync(X => X.Id == request.Id, cancellationToken);

            if (asset == null)
            {
                return Result.Failure(AssetErrors.AssetNotFound);
            }

            asset.IsDeleted = true;

            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
