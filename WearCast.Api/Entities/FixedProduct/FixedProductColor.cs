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
    public void AdjustSize(Size sizeName, int quantityChange)
    {
        var existingSize = Sizes.FirstOrDefault(s => s.Size == sizeName);

        if (existingSize != null)
        {
            existingSize.Quantity += quantityChange;
        }
        else
        {
            Sizes.Add(new FixedProductSize
            {
                Size = sizeName,
                Quantity = quantityChange
            });
            ReSortSizes();
        }
    }

    private void ReSortSizes()
    {
        var sorted = Sizes.OrderBy(s => s.Size).ToList();
        Sizes.Clear();
        foreach (var item in sorted) Sizes.Add(item);
    }
}