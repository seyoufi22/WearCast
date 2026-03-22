namespace WearCast.Api.Features.FixedProduct.GetFixedProductDetailsById.DTOs;

public record ProductDetailsImageDto
{
    public int Id { get; init; }
    public string ImageUrl { get; init; } = string.Empty;
}
