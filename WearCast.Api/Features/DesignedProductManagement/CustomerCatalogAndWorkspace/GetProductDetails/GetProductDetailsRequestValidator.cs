namespace WearCast.Api.Features.DesignedProductManagement.CustomerCatalogAndWorkspace.GetProductDetails
{
    public class GetProductDetailsRequestValidator : AbstractValidator<GetProductDetailsRequest>
    {
        public GetProductDetailsRequestValidator()
        {
            RuleFor(x => x.ProductId)
                .GreaterThan(0).WithMessage("Invalid Product Id");
        }
    }
}
