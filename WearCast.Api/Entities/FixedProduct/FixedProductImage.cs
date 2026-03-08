namespace WearCast.Api.Entities.FixedProduct;

public class FixedProductImage : BaseModel
{
    public int ProductColorId { get; set; }
    public string ImageUrl { get; set; }

    public FixedProductColor ProductColor { get; set; }
}
