namespace WearCast.Api.Features.DesignedProductManagement.Assets
{
    public static class AssetErrors
    {
        public static readonly Error AssetNotFound
            = new("AssetNotFound", "Asset does not exist.", StatusCodes.Status404NotFound);
    }
}
