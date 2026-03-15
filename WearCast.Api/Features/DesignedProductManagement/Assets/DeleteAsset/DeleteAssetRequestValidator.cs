namespace WearCast.Api.Features.DesignedProductManagement.Assets.DeleteAsset
{
    public class DeleteAssetRequestValidator : AbstractValidator<DeleteAssetRequest>
    {
        public DeleteAssetRequestValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Asset ID is required.");
        }
    }
}
