using WearCast.Api.Features.FixedProduct.GetFixedProductById.DTOs;

namespace WearCast.Api.Features.FixedProduct.GetFixedProductDetailsById.DTOs;

public record GetFixedProductDetailsByIdResponseDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public decimal Price { get; init; }
    public string Description { get; init; } = string.Empty;
    public string TargetAudience { get; init; } = string.Empty;
    public ProductDetailsCategoryDto Category { get; init; } = null!;
    public List<ProductDetailsColorDto> Colors { get; init; } = new();
    public List<ProductSizeDetailResponseDto> SizeDetails { get; init; } = new();
}
