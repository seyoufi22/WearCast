using WearCast.Api.Common.Enums;

namespace WearCast.Api.Entities.FixedProduct;

public class ProductSizeDetail
{
    public Size Size { get; set; }
    public decimal? A { get; set; }
    public decimal? B { get; set; }
    public decimal? C { get; set; }
}
