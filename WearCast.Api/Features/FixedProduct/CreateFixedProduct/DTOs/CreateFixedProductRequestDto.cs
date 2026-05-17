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
    public DressStyle DressStyle { get; init; }
    public TargetAudience TargetAudience { get; init; }

    public List<CreateProductSizeDetailDto> SizeDetails { get; init; } = new();

    [System.Text.Json.Serialization.JsonIgnore]
    public string CreatedById { get; set; } = string.Empty;

    [System.Text.Json.Serialization.JsonIgnore]
    public int SellerId { get; set; }
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

        RuleFor(x => x.DressStyle)
        .IsInEnum().WithMessage("Invalid DressStyle selected.");

        RuleFor(x => x.TargetAudience)
            .IsInEnum().WithMessage("Invalid TargetAudience selected.");

        RuleFor(x => x.SizeDetails)
            .NotEmpty().WithMessage("At least one size detail is required. Please add size measurements (A, B, C) for at least one size.")
            .Must(list => list.Select(d => d.Size).Distinct().Count() == list.Count)
            .WithMessage("Duplicate sizes are not allowed. Each size can only appear once.");

        RuleForEach(x => x.SizeDetails).ChildRules(detail =>
        {
            detail.RuleFor(d => d.Size)
                .IsInEnum().WithMessage("Invalid size value.");
        });
    }
}
