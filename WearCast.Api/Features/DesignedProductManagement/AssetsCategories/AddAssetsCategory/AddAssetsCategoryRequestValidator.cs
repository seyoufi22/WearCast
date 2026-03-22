namespace WearCast.Api.Features.DesignedProductManagement.AssetsCategories.AddAssetsCategory
{
    public class AddAssetsCategoryRequestValidator : AbstractValidator<AddAssetsCategoryRequest>
    {
        public AddAssetsCategoryRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Category name is required.")
                .MaximumLength(100).WithMessage("Category name must not exceed 100 characters.");
        }
    }
}
