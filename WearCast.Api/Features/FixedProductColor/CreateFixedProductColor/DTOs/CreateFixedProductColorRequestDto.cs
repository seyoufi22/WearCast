namespace WearCast.Api.Features.FixedProductColor.CreateFixedProductColor.DTOs;

public record CreateFixedProductColorRequestDto(
    int ProductId,
    string ColorName,
    string ColorCode,
    IFormFile Image,
    List<IFormFile>? AdditionalImages,
    [ModelBinder(BinderType = typeof(JsonModelBinder))]
    List<CreateSizeDto> Sizes 
) : IRequest<int>;

public record CreateSizeDto(Size Size, int Quantity);

public class CreateFixedProductColorValidator : AbstractValidator<CreateFixedProductColorRequestDto>
{
    private readonly ImageService _imageService;

    public CreateFixedProductColorValidator(ImageService imageService)
    {
        _imageService = imageService;

        RuleFor(x => x.ProductId).GreaterThan(0);
        RuleFor(x => x.ColorName).NotEmpty();
        RuleFor(x => x.ColorCode).NotEmpty();

        RuleFor(x => x.Image)
            .NotNull().WithMessage("Image is required.")
            .Must(img => _imageService.Validate(img).IsValid)
            .WithMessage(x => _imageService.Validate(x.Image).ErrorMessage);

        RuleForEach(x => x.AdditionalImages)
            .Must(img => _imageService.Validate(img).IsValid)
            .WithMessage((x, img) => _imageService.Validate(img).ErrorMessage);

        RuleFor(x => x.Sizes)
            .NotEmpty().WithMessage("At least one size is required.")
            .Must(sizes => sizes != null && sizes.Any()).WithMessage("Sizes cannot be empty.");

        RuleForEach(x => x.Sizes).ChildRules(size => {
            size.RuleFor(s => s.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than 0");
        });
    }
}