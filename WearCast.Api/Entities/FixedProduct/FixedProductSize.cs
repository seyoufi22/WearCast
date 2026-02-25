namespace WearCast.Api.Entities.FixedProduct;

public class FixedProductSize
{
    public int ProductColorId { get; set; }
    public Size Size { get; set; }
    public int Quantity { get; set; }

    public FixedProductColor ProductColor { get; set; }
}