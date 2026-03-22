namespace WearCast.Api.Features.DesignedProductManagement.AssetsCategories.UpdateAssetsCategory
{
    public class UpdateAssetsCategoryRequestValidator : AbstractValidator<UpdateAssetsCategoryRequest>
    {
        public UpdateAssetsCategoryRequestValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Invalid category ID.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Category name is required.")
                .MaximumLength(100).WithMessage("Category name must not exceed 100 characters.");
        }
    }
}
