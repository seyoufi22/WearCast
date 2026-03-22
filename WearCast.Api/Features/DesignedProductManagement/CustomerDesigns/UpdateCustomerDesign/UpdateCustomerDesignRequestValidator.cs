namespace WearCast.Api.Features.DesignedProductManagement.CustomerDesigns.UpdateCustomerDesign
{
    public class UpdateCustomerDesignRequestValidator : AbstractValidator<UpdateCustomerDesignRequest>
    {
        public UpdateCustomerDesignRequestValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Invalid Design Id");

            RuleFor(x => x.ViewDesignsJson)
                .NotEmpty();

            RuleFor(x => x.NewProductColorId)
                .GreaterThan(0).WithMessage("Invalid Product Color Id");
        }
    }
}
