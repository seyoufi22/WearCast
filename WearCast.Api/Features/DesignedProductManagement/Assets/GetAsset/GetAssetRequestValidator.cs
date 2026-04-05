namespace WearCast.Api.Features.DesignedProductManagement.Assets.GetAsset
{
    public class GetAssetRequestValidator : AbstractValidator<GetAssetRequest>
    {
        public GetAssetRequestValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("ID is required.");
        }
    }
}
