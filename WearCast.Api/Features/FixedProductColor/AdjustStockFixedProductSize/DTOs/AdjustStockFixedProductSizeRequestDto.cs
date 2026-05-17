namespace WearCast.Api.Features.FixedProductSize.AdjustStockFixedProductSize.DTOs;

public class AdjustStockFixedProductSizeRequestDto 
{
    public int ColorId { get; set; }
    public List<SizeAdjustmentDto> Adjustments { get; set; } = new();
}

public class SizeAdjustmentDto
{
    public Size Size { get; set; }
    public int Quantity { get; set; }
}