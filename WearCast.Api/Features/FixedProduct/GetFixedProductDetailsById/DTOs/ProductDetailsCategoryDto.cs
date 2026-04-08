namespace WearCast.Api.Features.FixedProduct.GetFixedProductDetailsById.DTOs;

public record ProductDetailsCategoryDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string ImageUrl { get; init; } = string.Empty;
}
