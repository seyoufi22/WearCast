namespace WearCast.Api.Entities.FixedProduct;

public class FixedProduct : BaseModel
{
    public int CategoryId { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
    public TargetAudience TargetAudience { get; set; }

    public Category Category { get; set; }
    public ICollection<FixedProductColor> Colors { get; set; }
    public ICollection<ProductSizeDetail> SizeDetails { get; set; } = new List<ProductSizeDetail>();
}