namespace WearCast.Api.Features.FixedProduct.GetFixedProductDetailsById.DTOs;

public record ProductDetailsSizeDto
{
    public string Size { get; init; } = string.Empty;
    public int Quantity { get; init; }
}
