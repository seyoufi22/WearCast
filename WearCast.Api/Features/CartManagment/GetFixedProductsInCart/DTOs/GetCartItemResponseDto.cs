namespace WearCast.Api.Features.CartManagment.GetFixedProductsInCart.DTOs;

public class GetCartItemResponseDto
{
    public bool unavailable { get; set; } = false;
    public int CartItemId { get; set; }
    public int ProductId { get; set; }
    public int ProductColorId { get; set; }
    public string ProductName { get; set; }
    public decimal Price { get; set; }    
    public string Image { get; set; }
    public List<SizeDto> Sizes { get; set; }
}
public record SizeDto(
    Size Size,
    int QuantityInCart,     
    int QuantityAvailable  
);