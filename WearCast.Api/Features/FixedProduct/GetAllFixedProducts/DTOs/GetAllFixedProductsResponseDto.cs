namespace WearCast.Api.Features.FixedProduct.GetAllFixedProducts.DTOs;

public record GetAllFixedProductsResponseDto
{
    public int Id { get; init; }
    public int CategoryId { get; init; }
    public string Name { get; init; } = string.Empty;
    public decimal Price { get; init; }
    public string Description { get; init; } = string.Empty;
    public string TargetAudience { get; init; } = string.Empty;
    
    public List<ProductSizeDetailGetAllResponseDto> SizeDetails { get; init; } = new();
}

public record ProductSizeDetailGetAllResponseDto
{
    public string Size { get; init; } = string.Empty;
    public decimal? A { get; init; }
    public decimal? B { get; init; }
    public decimal? C { get; init; }
}
