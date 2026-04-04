namespace WearCast.Api.Features.FixedProductColor.AddFixedProductImage.DTOs;

public record AddFixedProductImageCommandDto(int ProductColorId, IFormFile Image, int sellerId, bool isAdminRequest) : IRequest<Result>;
public class AddFixedProductImageValidator : AbstractValidator<AddFixedProductImageCommandDto>
{
    public AddFixedProductImageValidator()
    {

        RuleFor(x => x.ProductColorId)
            .GreaterThan(0).WithMessage("ProductColorId must be greater than 0.");

        RuleFor(x => x.Image)
            .NotNull().WithMessage("Image is required.");
    }
}