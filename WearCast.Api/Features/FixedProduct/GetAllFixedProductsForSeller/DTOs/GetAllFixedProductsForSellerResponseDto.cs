namespace WearCast.Api.Features.FixedProduct.GetAllFixedProductsForSeller.DTOs;

public record GetAllFixedProductsForSellerResponseDto { 
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public TargetAudience TargetAudience { get; init; } = TargetAudience.Unisex;
    public decimal Price { get; init; }
    public int Stock { get; init; }
    public bool IsRejected { get; init; }
    public string? MainImageUrl { get; init; }

}
