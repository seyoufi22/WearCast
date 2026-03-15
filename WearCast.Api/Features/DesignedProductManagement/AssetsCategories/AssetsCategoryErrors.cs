namespace WearCast.Api.Features.DesignedProductManagement.AssetsCategories
{
    public static class AssetsCategoryErrors
    {
        public static readonly Error CategoryAlreadyExists =
             new("AssetsCategory.AlreadyExists", "A category with this name already exists.", StatusCodes.Status400BadRequest);

        public static readonly Error CategoryNotFound =
             new("AssetsCategory.NotFound", "Assets Category does not exist.", StatusCodes.Status404NotFound);
    }
}
