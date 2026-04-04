namespace WearCast.Api.Entities.FixedProduct;

using WearCast.Api.Entities.BusinessActors;

public class Favourite
{
    public int CustomerId { get; set; }
    public Customer Customer { get; set; }

    public int FixedProductColorId { get; set; }
    public FixedProductColor FixedProductColor { get; set; }
}
