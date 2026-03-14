namespace WearCast.Api.Features.DesignedProductManagement.FactoryProducts.DeleteDesignedProduct
{
    public class DeleteDesignedProductRequestValidator : AbstractValidator<DeleteDesignedProductRequest>
    {
        public DeleteDesignedProductRequestValidator()
        {
            RuleFor(x => x.Slug)
                .NotEmpty().WithMessage("Product slug is required.");
        }
    }
}
