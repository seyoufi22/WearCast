namespace WearCast.Api.Features.FixedProductSize.AdjustStockFixedProductSize.DTOs;

public class AdjustStockFixedProductSizeRequestDto 
{
    public int ColorId { get; set; }
    public Size Size { get; set; }
    public int Quantity { get; set; }
}

