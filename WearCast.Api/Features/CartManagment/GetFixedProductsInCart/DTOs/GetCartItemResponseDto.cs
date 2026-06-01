namespace WearCast.Api.Features.CartManagment.GetFixedProductsInCart.DTOs;

// 1. The new wrapper class that holds the items list and the grand total
public class FixedCartSummaryDto
{
    public List<GetCartItemResponseDto> Items { get; set; } = new();
    public decimal TotalFixedProductsPrice { get; set; } // The grand total price for all fixed products combined
}

// 2. The item class with the newly added properties
public class GetCartItemResponseDto
{
    public bool unavailable { get; set; } = false;
    public int CartItemId { get; set; }
    public int ProductId { get; set; }
    public int ProductColorId { get; set; }
    public string ProductName { get; set; }
    public decimal Price { get; set; }
    public string Image { get; set; }
    public int TotalQuantity { get; set; } // Total number of pieces for this specific product
    public decimal SubTotal { get; set; }  // Total price for this product (Quantity * Price)


    public List<SizeDto> Sizes { get; set; }
}

public record SizeDto(
    Size Size,
    int QuantityInCart,
    int QuantityAvailable
);