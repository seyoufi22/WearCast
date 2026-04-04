namespace WearCast.Api.Features.DesignedProductManagement.Assets.GetAllAssets
{
    public class GetAllAssetsRequestValidator : AbstractValidator<GetAllAssetsRequest>
    {
        public GetAllAssetsRequestValidator()
        {
            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("Category ID is required.");
        }
    }
}
