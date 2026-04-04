namespace WearCast.Api.Features.FixedProduct.GetFixedProductDetailsById.DTOs;

public record ProductDetailsColorDto
{
    public int Id { get; init; }
    public string ColorName { get; init; } = string.Empty;
    public string ColorCode { get; init; } = string.Empty;
    public string ImageUrl { get; init; } = string.Empty;
    public List<ProductDetailsImageDto> Images { get; init; } = new();
    public List<ProductDetailsSizeDto> AvailableSizes { get; init; } = new();
}
