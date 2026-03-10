using System.Text.Json;
using System.Text.Json.Serialization;
using FluentValidation;
namespace WearCast.Api.Features.FixedProductColor.CreateFixedProductColor.DTOs;

public record CreateFixedProductColorRequestDto(
    int ProductId,
    string ColorName,
    string ColorCode,
    IFormFile Image,
    List<IFormFile>? AdditionalImages,
    [ModelBinder(BinderType = typeof(JsonModelBinder))] List<CreateSizeDto> Sizes
) : IRequest<int>;

public class CreateSizeDto
{
    [JsonPropertyName("size")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Size Size { get; set; }

    [JsonPropertyName("quantity")]
    public int Quantity { get; set; }
}

public class CreateFixedProductColorValidator : AbstractValidator<CreateFixedProductColorRequestDto>
{
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter() }
    };

    private readonly ImageService _imageService;
    private readonly IRepository<Entities.FixedProduct.FixedProduct> _productRepo;
    private readonly IRepository<Entities.FixedProduct.FixedProductColor> _colorRepo;

    public CreateFixedProductColorValidator(
        ImageService imageService,
        IRepository<Entities.FixedProduct.FixedProduct> productRepo,
        IRepository<Entities.FixedProduct.FixedProductColor> colorRepo)
    {
        _imageService = imageService;
        _productRepo = productRepo;
        _colorRepo = colorRepo;

        RuleFor(x => x.ProductId)
            .GreaterThan(0)
            .MustAsync(async (id, cancellation) => 
            {
                var product = await _productRepo.GetAsync(e => e.Id == id);
                return product != null;
            })
            .WithMessage("Product not found.");

        RuleFor(x => x.ColorName.Trim()).NotEmpty()
            .MaximumLength(50).WithMessage("Color name cannot exceed 50 characters.")
            .Must(name => !string.IsNullOrWhiteSpace(name))
            .WithMessage("Color name cannot be empty spaces.");  

        RuleFor(x => x.ColorCode.Trim())
            .NotEmpty().WithMessage("Color code is required.")
            .Matches("^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$")
            .WithMessage("Invalid color code format. It should be a valid Hex code (e.g., #FFFFFF or #FFF).")
            .MaximumLength(20).WithMessage("Color code is too long.")
            .MustAsync(async (dto, code, cancellation) =>
            {
                var normalizedCode = code.ToUpper();
                var existingColor = await _colorRepo.GetAsync(c => 
                    c.IsDeleted == false &&
                    c.ProductId == dto.ProductId &&
                    c.ColorCode == normalizedCode);
                return existingColor == null;
            })
            .WithMessage("A color with this Hex Code already exists for this product.");

        RuleFor(x => x.Image)
            .NotNull().WithMessage("Image is required.")
            .Must(img => _imageService.Validate(img).IsValid)
            .WithMessage(x => _imageService.Validate(x.Image).ErrorMessage);

        RuleForEach(x => x.AdditionalImages)
            .Must(img => _imageService.Validate(img).IsValid)
            .WithMessage((x, img) => _imageService.Validate(img).ErrorMessage);

        RuleFor(x => x.Sizes)
            .NotEmpty().WithMessage("Sizes are required.")
            .Must(sizes => sizes != null && sizes.All(s => s.Quantity > 0))
            .WithMessage("Quantity must be greater than 0 for all sizes.")
            .Must(sizes => sizes == null || sizes.Select(s => s.Size).Distinct().Count() == sizes.Count)
            .WithMessage("Sizes must be unique. You cannot add the same size multiple times.");

        RuleForEach(x => x.Sizes).ChildRules(size =>
            {
                size.RuleFor(s => s.Size)
                    .IsInEnum()
                    .WithMessage("One or more selected sizes are invalid.");

                size.RuleFor(s => s.Quantity)
                    .GreaterThan(0)
                    .WithMessage("Quantity must be greater than 0.");
            });
    }
}