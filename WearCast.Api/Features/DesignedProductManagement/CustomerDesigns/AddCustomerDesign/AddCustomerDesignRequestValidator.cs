namespace WearCast.Api.Features.DesignedProductManagement.CustomerDesigns.AddCustomerDesign
{
    public class AddCustomerDesignRequestValidator : AbstractValidator<AddCustomerDesignRequest>
    {
        public AddCustomerDesignRequestValidator()
        {
            RuleFor(x => x.ViewDesignsJson)
                .NotEmpty();

            RuleFor(x => x.ProductId)
                .GreaterThan(0).WithMessage("Invalid Product Id");

            RuleFor(x => x.ProductColorId)
                .GreaterThan(0).WithMessage("Invalid Product Color Id");
        }
    }
}
