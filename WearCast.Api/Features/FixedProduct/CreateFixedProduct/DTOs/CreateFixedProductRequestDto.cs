using WearCast.Api.Common.Enums;
using FluentValidation;
using WearCast.Api.Abstractions;

namespace WearCast.Api.Features.FixedProduct.CreateProduct.DTOs;

public record CreateFixedProductRequestDto : IRequest<Result<CreateFixedProductResponseDto>>
{
    public int CategoryId { get; init; }
    public string Name { get; init; } = string.Empty;
    public decimal Price { get; init; }
    public string Description { get; init; } = string.Empty;
    public TargetAudience TargetAudience { get; init; }

    public List<CreateProductSizeDetailDto> SizeDetails { get; init; } = new();

    [System.Text.Json.Serialization.JsonIgnore]
    public string CreatedById { get; set; } = string.Empty;
}

public record CreateProductSizeDetailDto
{
    public Size Size { get; init; }
    public decimal? A { get; init; }
    public decimal? B { get; init; }
    public decimal? C { get; init; }
}

public class CreateFixedProductValidator : AbstractValidator<CreateFixedProductRequestDto>
{
    public CreateFixedProductValidator()
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
