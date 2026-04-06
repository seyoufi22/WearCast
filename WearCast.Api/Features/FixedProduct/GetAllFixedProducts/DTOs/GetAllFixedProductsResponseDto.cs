using WearCast.Api.Features.FixedProductColor.GetFixedProductColorById;

namespace WearCast.Api.Features.FixedProduct.GetAllFixedProducts.DTOs;

public record GetAllFixedProductsResponseDto
{
    public int Id { get; init; }
    public int colorId { get; set; }
    public int CategoryId { get; init; }
    public string Name { get; init; } = string.Empty;
    public decimal Price { get; init; }
    public string Description { get; init; } = string.Empty;
    public TargetAudience? TargetAudience { get; init; } = null;
    public string? MainImageUrl { get; init; }
    public int SellerId { get; init; }
    
    public List<ProductSizeDetailGetAllResponseDto> SizeDetails { get; init; } = new();
}

public record ProductSizeDetailGetAllResponseDto
{
    public string Size { get; init; } = string.Empty;
    public decimal? A { get; init; }
    public decimal? B { get; init; }
    public decimal? C { get; init; }
}
