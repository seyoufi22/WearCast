using WearCast.Api.Common.Enums;
using WearCast.Api.Abstractions;

namespace WearCast.Api.Features.FixedProduct.GetFixedProductById.DTOs;

public record GetFixedProductByIdResponseDto
{
    public int Id { get; init; }
    public int CategoryId { get; init; }
    public string Name { get; init; } = string.Empty;
    public decimal Price { get; init; }
    public string Description { get; init; } = string.Empty;
    public string TargetAudience { get; init; } = string.Empty;
    
    public List<ProductSizeDetailResponseDto> SizeDetails { get; init; } = new();
}

public record ProductSizeDetailResponseDto
{
    public string Size { get; init; } = string.Empty;
    public decimal? A { get; init; }
    public decimal? B { get; init; }
    public decimal? C { get; init; }
}
