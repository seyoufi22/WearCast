namespace WearCast.Api.Features.FixedProductColor.UpdateFixedProductColor.DTOs;

public class UpdateFixedProductColorRequestDto : IRequest<bool>
{
    public int ColorId { get; set; }
    public string ColorName { get; set; }
    public string ColorCode { get; set; }
    public IFormFile? Image { get; set; }
}

public class UpdateFixedProductColorValidator : AbstractValidator<UpdateFixedProductColorRequestDto>
{
    private readonly ImageService _imageService;
    private readonly IRepository<Entities.FixedProduct.FixedProduct> _productRepo;
    private readonly IRepository<Entities.FixedProduct.FixedProductColor> _colorRepo;

    public UpdateFixedProductColorValidator(
        ImageService imageService,
        IRepository<Entities.FixedProduct.FixedProduct> productRepo,
        IRepository<Entities.FixedProduct.FixedProductColor> colorRepo)
    {
        _imageService = imageService;
        _productRepo = productRepo;
        _colorRepo = colorRepo;

        RuleFor(x => x.ColorId).GreaterThan(0)
            .MustAsync(async (id, cancellation) =>
            {
                var color = await _colorRepo.GetAsync(c => c.Id == id);
                return color != null;
            })
            .WithMessage("Product color not found.");

        RuleFor(x => x.ColorName.Trim()).NotEmpty().WithMessage("Color name is required.")
            .MaximumLength(50).WithMessage("Color name cannot exceed 50 characters.")
            .Must(name => !string.IsNullOrWhiteSpace(name))
            .WithMessage("Color name cannot be empty spaces.");

        RuleFor(x => x.ColorCode.Trim())
            .NotEmpty()
            .Matches("^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$")
            .WithMessage("Invalid color code format. It should be a valid Hex code (e.g., #FFFFFF).")
            .MaximumLength(20)
        .MustAsync(async (dto, code, cancellation) =>
         {

             var currentColor = await _colorRepo.GetAsync(c => c.Id == dto.ColorId);

             if (currentColor == null) return true;

             var normalizedCode = code.Trim().ToUpper();

             var existingColor = await _colorRepo.GetAsync(c =>
                 c.IsDeleted == false &&
                 c.ProductId == currentColor.ProductId &&
                 c.ColorCode.ToUpper() == normalizedCode &&
                 c.Id != dto.ColorId);

             return existingColor == null;
         })
            .WithMessage("A color with this Hex Code already exists for this product.");
        When(x => x.Image != null, () =>
        {
            RuleFor(x => x.Image!)
                .Must(img => _imageService.Validate(img).IsValid)
                .WithMessage(x => _imageService.Validate(x.Image!).ErrorMessage);
        });
    }
}