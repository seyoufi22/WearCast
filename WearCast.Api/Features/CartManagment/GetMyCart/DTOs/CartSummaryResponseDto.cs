namespace WearCast.Api.Features.CartManagment.GetMyCart.DTOs;

// 1. الكلاس الأساسي اللي بيشمل العربة كلها
public class CartSummaryResponseDto
{
    // ليستة المنتجات الثابتة
    public List<GetFixedCartItemDto> FixedItems { get; set; } = new();

    // ليستة المنتجات المصممة
    public List<GetDesignedCartItemDto> DesignedItems { get; set; } = new();

    // حساب الإجمالي للمنتجات كلها (الثابتة + المصممة) تلقائياً
    public decimal SubTotal =>
        (FixedItems?.Sum(item => item.Price * item.TotalQuantity) ?? 0) +
        (DesignedItems?.Sum(item => item.Price * item.TotalQuantity) ?? 0);

    public decimal DeliveryFee { get; set; }  // مصاريف الشحن

    // الإجمالي النهائي تلقائياً
    public decimal GrandTotal => SubTotal + DeliveryFee;
}

// 2. DTO المنتجات الثابتة
public class GetFixedCartItemDto
{
    public bool unavailable { get; set; }
    public int CartItemId { get; set; }
    public int ProductId { get; set; }
    public int ProductColorId { get; set; }
    public string ProductName { get; set; }
    public decimal Price { get; set; }
    public string Image { get; set; }
    public List<FixedSizeDto> Sizes { get; set; } = new();

    public int TotalQuantity => Sizes != null ? Sizes.Sum(s => s.QuantityInCart) : 0;
}

// 3. DTO المنتجات المتصممة
public class GetDesignedCartItemDto
{
    public bool unavailable { get; set; }
    public int CartItemId { get; set; }
    public int? CustomerDesignedId { get; set; }
    public string? ProductName { get; set; }
    public decimal Price { get; set; }
    public string? PriceDescription { get; set; }
    public string? Image { get; set; }
    public List<DesignedSizeDto> Sizes { get; set; } = new();

    public int TotalQuantity => Sizes != null ? Sizes.Sum(s => s.QuantityInCart) : 0;
}

// 4. مقاسات المنتجات الثابتة (فيها الكمية المتاحة)
public record FixedSizeDto(Size Size, int QuantityInCart, int QuantityAvailable);

// 5. مقاسات المنتجات المصممة
public record DesignedSizeDto(Size Size, int QuantityInCart);