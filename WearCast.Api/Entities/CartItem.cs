namespace WearCast.Api.Entities;
using WearCast.Api.Entities.FixedProduct;
using WearCast.Api.Entities.BusinessActors;
public class CartItem
{
    public int ColorId { get; set; }
    public int CustomerId { get; set; }
    public FixedProductColor Color { get; set; }
    public Customer Customer { get; set; } 
    public ICollection<FixedProductSize> Sizes { get; set; } = new List<FixedProductSize>();
}
