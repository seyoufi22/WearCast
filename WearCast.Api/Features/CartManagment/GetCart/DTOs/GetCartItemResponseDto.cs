namespace WearCast.Api.Features.CartManagment.GetCart.DTOs;

public class GetCartItemResponseDto
{
    public int IdCartItem { get; set; }
    public int IdProduct { get; set; }
    public int IdProductColor { get; set; }
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