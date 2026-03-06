namespace WearCast.Api.Entities.FixedProduct;

public class FixedProductColor : BaseModel
{
    public int ProductId { get; set; }
    public string ColorName { get; set; }
    public string ColorCode { get; set; }
    public string ImageUrl { get; set; }

    public FixedProduct Product { get; set; }
    public ICollection<FixedProductImage> Images { get; set; }
    public ICollection<FixedProductSize> Sizes { get; set; }= new List<FixedProductSize>();
}