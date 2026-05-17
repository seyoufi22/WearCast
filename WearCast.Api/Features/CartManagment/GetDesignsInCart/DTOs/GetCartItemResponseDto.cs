namespace WearCast.Api.Features.CartManagment.GetDesignsInCart.DTOs;
public class GetCartItemResponseDto
{
    public bool unavailable   { get; set; } = false;
    public int CartItemId { get; set; }
    public int? CustomerDesignedId {  get; set; }
    public string? ProductName { get; set; }
    public decimal Price { get; set; }
    public string? Image { get; set; }
    public List<SizeDto> Sizes { get; set; } = new List<SizeDto>();
}
public record SizeDto(
    Size Size,
    int QuantityInCart
);