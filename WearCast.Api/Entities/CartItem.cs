namespace WearCast.Api.Entities;
using WearCast.Api.Entities.FixedProduct;
using WearCast.Api.Entities.BusinessActors;
public class CartItem : BaseModel
{
    public int? FixedColorId { get; set; }
    public int? CustomerDesignId { get; set; } 
    public int CustomerId { get; set; }
    public FixedProductColor? FixedColor { get; set; }
    public CustomerDesign? DesignedCustomer { get; set; }
    public Customer Customer { get; set; } = null!;
    public ICollection<FixedProductSize> Sizes { get; set; } = new List<FixedProductSize>();
    public void AddOrUpdateSize(Size sizeName, int quantityChange)
    {
        var existingSize = Sizes.FirstOrDefault(s => s.Size == sizeName);

        if (existingSize != null)
        {
            existingSize.Quantity += quantityChange;

            if (existingSize.Quantity <= 0)
            {
                Sizes.Remove(existingSize);
            }
        }
        else
        {
            if (quantityChange > 0)
            {
                Sizes.Add(new FixedProductSize
                {
                    Size = sizeName,
                    Quantity = quantityChange
                });
            }
        }
        var sortedSizes = Sizes.OrderBy(s => s.Size).ToList();

        Sizes.Clear();

        foreach (var size in sortedSizes)
        {
            Sizes.Add(size);
        }
    }
}
