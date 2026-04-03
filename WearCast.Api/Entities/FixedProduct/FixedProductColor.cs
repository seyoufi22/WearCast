using System.Drawing;

namespace WearCast.Api.Entities.FixedProduct;

public class FixedProductColor : BaseModel
{
    public int ProductId { get; set; }
    public string ColorName { get; set; } = default!;
    public string ColorCode { get; set; } = default!;
    public string ImageUrl { get; set; } = default!;

    public FixedProduct Product { get; set; } = default!;
    public ICollection<FixedProductImage> Images { get; set; } = default!;
    public ICollection<FixedProductSize> Sizes { get; set; }= new List<FixedProductSize>();
    public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    public ICollection<Favourite> Favourites { get; set; } = new List<Favourite>();
    public void AddSize(FixedProductSize  newSize)
    {
        Sizes.Add(newSize);
        var sortedList = Sizes
            .OrderBy(s => s.Size)
            .Select(s => new FixedProductSize
            {
                Size = s.Size,
                Quantity = s.Quantity
            })
            .ToList();
        Sizes.Clear();
        foreach (var item in sortedList)
        {
            Sizes.Add(item);
        }
    }
}