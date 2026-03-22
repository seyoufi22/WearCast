namespace WearCast.Api.Features.DesignedProductManagement.FactoryProducts.DeleteDesignedProduct
{
    public class DeleteDesignedProductRequestValidator : AbstractValidator<DeleteDesignedProductRequest>
    {
        public DeleteDesignedProductRequestValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Invalid Product Id.");
        }
    }
}
