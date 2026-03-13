namespace WearCast.Api.Features.DesignedProductManagement.AssetsCategories.DeleteAssetsCategory
{
    public class DeleteAssetsCategoryRequestValidator : AbstractValidator<DeleteAssetsCategoryRequest>
    {
        public DeleteAssetsCategoryRequestValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Invalid category ID.");
        }
    }
}
