namespace WearCast.Api.Features.DesignedProductManagement.Assets.GetAsset
{
    public class GetAssetHandler(ApplicationDbContext context) : IRequestHandler<GetAssetRequest, Result<GetAssetResponse>>
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<Result<GetAssetResponse>> Handle(GetAssetRequest request, CancellationToken cancellationToken)
        {
            var asset = await _context.DesignAssets
                .AsNoTracking()
                .Where(a => a.Id == request.Id)
                .Select(a => new GetAssetResponse(
                    a.Id,
                    a.Name,
                    a.ImageUrl,
                    a.WidthPx,
                    a.HeightPx,
                    a.DesignAssetCategoryId
                    ))
                .FirstOrDefaultAsync(cancellationToken);

            if (asset is null)
            {
                return Result.Failure<GetAssetResponse>(AssetErrors.AssetNotFound);
            }

            return Result.Success(asset);
        }
    }
}
