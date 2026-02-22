namespace WearCast.Api.Entities.FixedProduct;

public class FixedProductSizeDetails : BaseModel
{
    public int ProductId { get; set; }
    public Size Size { get; set; }
    public int A { get; set; }
    public int B { get; set; }
    public int C { get; set; }

    public FixedProduct Product { get; set; }
}