namespace WearCast.Api.Features.FixedProduct.GetAllFixedProductsStatusForSeller.DTOs;

public record GetAllFixedProductsStatusForSellerResponseDto
{
    public int TotalProducts { get; init; } = 0;
    public int Approved { get; init; } = 0;
    public int LowStock { get; init; } = 0;
    public int Rejected { get; init; } = 0;
    public int OutOfStock { get; init; } = 0;
}
