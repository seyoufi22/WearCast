namespace WearCast.Api.Features.CartManagment.GetDesignsInCart.DTOs;

public class CartSummaryResponseDto
{
    public List<GetCartItemResponseDto> Items { get; set; } = new();
    public decimal SubTotal { get; set; }     // مجموع المنتجات فقط
    public decimal DeliveryFee { get; set; }  // مصاريف الشحن
    public decimal GrandTotal { get; set; }   // الإجمالي النهائي (المنتجات + الشحن)
}

public class GetCartItemResponseDto
{
    public bool unavailable { get; set; } = false;
    public int CartItemId { get; set; }
    public int? CustomerDesignedId { get; set; }
    public string? ProductName { get; set; }
    public decimal Price { get; set; }
    public string? PriceDescription { get; set; } 
    public string? Image { get; set; }
    public List<SizeDto> Sizes { get; set; } = new List<SizeDto>();

    public int TotalQuantity => Sizes != null ? Sizes.Sum(s => s.QuantityInCart) : 0;
}

public record SizeDto(Size Size, int QuantityInCart);