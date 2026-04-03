namespace WearCast.Api.Entities.FixedProduct;

public class FixedProduct : BaseModel
{
    public int CategoryId { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Description { get; set; } = string.Empty;
    public TargetAudience TargetAudience { get; set; }

    public Category Category { get; set; } = default!;
    public int SellerId { get; set; }
    public BusinessActors.Seller Seller { get; set; } = default!;
    public ICollection<FixedProductColor> Colors { get; set; } = default!;
    public ICollection<ProductSizeDetail> SizeDetails { get; set; } = new List<ProductSizeDetail>();
}