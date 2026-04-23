namespace WearCast.Api.Features.FixedProduct.GetAllFixedProductsForAdmin.DTOs;

public record GetAllFixedProductsForAdminResponseDto
{
    public int Id { get; init; }
    public string ProductName { get; init; } = string.Empty;
    public string StoreName { get; init; } = string.Empty;

    public decimal Price { get; init; }
    public string? MainImageUrl { get; init; }

    public bool IsRejected { get; init; }
}