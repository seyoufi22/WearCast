namespace WearCast.Api.Features.FixedProductColor.AddFixedProductImage.DTOs;

public record AddFixedProductImageRequestDto(int ProductColorId,IFormFile File) : IRequest<bool>;
public class AddFixedProductImageValidator : AbstractValidator<AddFixedProductImageRequestDto>
{
    private readonly ImageService _imageService;

    public AddFixedProductImageValidator(ImageService imageService)
    {
        _imageService = imageService;

        RuleFor(x => x.ProductColorId)
            .GreaterThan(0).WithMessage("ProductColorId must be greater than 0.");

        RuleFor(x => x.File)
            .NotNull().WithMessage("Image is required.")
            .Must(file => _imageService.Validate(file).IsValid)
            .WithMessage(x => _imageService.Validate(x.File).ErrorMessage);
    }
}