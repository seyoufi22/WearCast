namespace WearCast.Api.Features.DesignedProductManagement.FactoryProducts.UpdateDesignedProduct
{
    public class UpdateDesignedProductRequestValidator : AbstractValidator<UpdateDesignedProductRequest>
    {
        public UpdateDesignedProductRequestValidator()
        {
            RuleFor(x => x.CurrentSlug).NotEmpty().WithMessage("Product slug is required.");

            RuleFor(x => x.Name)
                 .NotEmpty().WithMessage("Product name is required.")
                 .MaximumLength(200).WithMessage("Product name must not exceed 200 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("Description is too long.");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than zero.");

            RuleFor(x => x.CanvasWidth)
                .GreaterThan(0).WithMessage("Canvas width must be a valid positive number.");

            RuleFor(x => x.CanvasHeight)
                .GreaterThan(0).WithMessage("Canvas height must be a valid positive number.");

            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("Category ID is required.");

            RuleFor(x => x.TargetAudience)
                .IsInEnum().WithMessage("Invalid target audience value.");
        }
    }
}
