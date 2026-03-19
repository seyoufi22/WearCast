namespace WearCast.Api.Features.DesignedProductManagement.FactoryProducts.UpdateDesignedProduct
{
    public class UpdateDesignedProductRequestValidator : AbstractValidator<UpdateDesignedProductRequest>
    {
        public UpdateDesignedProductRequestValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Invalid Product Id.");

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

            RuleFor(x => x.TargetAudiences)
                .NotEmpty().WithMessage("At least one target audience must be selected.");

            // بنتأكد إن كل عنصر جوه الليستة هو قيمة صحيحة في الـ Enum
            RuleForEach(x => x.TargetAudiences)
                .IsInEnum().WithMessage("Invalid target audience value.");
        }
    }
}
