namespace WearCast.Api.Features.DesignedProductManagement.CustomerCatalogAndWorkspace.GetProductDetails
{
    public class GetProductDetailsRequestValidator : AbstractValidator<GetProductDetailsRequest>
    {
        public GetProductDetailsRequestValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Invalid Product Id");
        }
    }
}
