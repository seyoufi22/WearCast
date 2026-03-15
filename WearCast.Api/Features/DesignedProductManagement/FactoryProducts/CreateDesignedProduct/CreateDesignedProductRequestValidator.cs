namespace WearCast.Api.Features.DesignedProductManagement.FactoryProducts.CreateDesignedProduct
{
    public class CreateDesignedProductRequestValidator : AbstractValidator<CreateDesignedProductRequest>
    {
        public CreateDesignedProductRequestValidator()
        {
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
                .GreaterThan(0).WithMessage("Invalid Category ID .");

            RuleFor(x => x.TargetAudience)
                .IsInEnum().WithMessage("Invalid target audience value.");


            /*
            RuleFor(x => x.SizeDetails)
                .NotEmpty().WithMessage("You must add at least one size for the product.");

            */

            RuleForEach(x => x.SizeDetails)
                  .SetValidator(new CreateProductSizeRequestValidator());
        }

    }
    public class CreateProductSizeRequestValidator : AbstractValidator<CreateProductSizeRequest>
    {
        public CreateProductSizeRequestValidator()
        {
            RuleFor(x => x.Size)
            .IsInEnum().WithMessage("Invalid size selected.");

            RuleFor(x => x.A)
            .GreaterThan(0).WithMessage("Measurement A must be greater than 0.")
            .LessThanOrEqualTo(200).WithMessage("Measurement A is unusually large.")
            .When(x => x.A.HasValue);

            RuleFor(x => x.B)
                .GreaterThan(0).WithMessage("Measurement B must be greater than 0.")
                .LessThanOrEqualTo(200).WithMessage("Measurement B is unusually large.")
                .When(x => x.B.HasValue);

            RuleFor(x => x.C)
                .GreaterThan(0).WithMessage("Measurement C must be greater than 0.")
                .LessThanOrEqualTo(200).WithMessage("Measurement C is unusually large.")
                .When(x => x.C.HasValue);

            RuleFor(x => x)
                .Must(x => x.A.HasValue || x.B.HasValue || x.C.HasValue)
                .WithMessage("At least one measurement (A, B, or C) must be provided for each size.");
        }
    }

}
