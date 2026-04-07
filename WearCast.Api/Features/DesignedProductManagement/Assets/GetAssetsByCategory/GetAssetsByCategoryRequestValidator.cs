namespace WearCast.Api.Features.DesignedProductManagement.Assets.GetAssetsByCategory
{
    public class GetAssetsByCategoryRequestValidator : AbstractValidator<GetAssetsByCategoryRequest>
    {
        public GetAssetsByCategoryRequestValidator()
        {
            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("Category ID is required.");
        }
    }
}
