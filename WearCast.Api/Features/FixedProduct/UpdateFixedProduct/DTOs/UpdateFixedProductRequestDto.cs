using WearCast.Api.Common.Enums;
using FluentValidation;
using WearCast.Api.Abstractions;
using WearCast.Api.Entities.FixedProduct;

namespace WearCast.Api.Features.FixedProduct.UpdateFixedProduct.DTOs;

public record UpdateFixedProductRequestDto : IRequest<Result<UpdateFixedProductResponseDto>>
{
    public int Id { get; set; }
    
    public int CategoryId { get; init; }
    public string Name { get; init; } = string.Empty;
    public decimal Price { get; init; }
    public string Description { get; init; } = string.Empty;
    public TargetAudience TargetAudience { get; init; }
    public List<UpdateProductSizeDetailDto> SizeDetails { get; init; } = new();
}

public record UpdateProductSizeDetailDto
{
    public Size Size { get; init; }
    public decimal? A { get; init; }
    public decimal? B { get; init; }
    public decimal? C { get; init; }
}

public record UpdateFixedProductResponseDto(int Id, string Name);

public class UpdateFixedProductValidator : AbstractValidator<UpdateFixedProductRequestDto>
{
    public UpdateFixedProductValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Product name is required.")
            .MaximumLength(150).WithMessage("Product name must not exceed 150 characters.");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than zero.");

        RuleFor(x => x.CategoryId)
            .GreaterThan(0).WithMessage("Valid CategoryId is required.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.");
            
        RuleFor(x => x.TargetAudience)
            .IsInEnum().WithMessage("Valid TargetAudience is required.");
    }
}
