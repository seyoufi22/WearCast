namespace WearCast.Api.Features.DesignedProductManagement.FactoryProductSizes.AddFactoryProductSize
{
    public class AddFactoryProductSizeRequestValidator : AbstractValidator<AddFactoryProductSizeRequest>
    {
        public AddFactoryProductSizeRequestValidator()
        {
            RuleFor(x => x.Size).IsInEnum().WithMessage("Invalid size selected.");

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
                .WithMessage("At least one measurement (A, B, or C) must be provided.");

            RuleFor(x => x.ProductId).GreaterThan(0).WithMessage("Invalid Product Id.");
        }
    }
}
