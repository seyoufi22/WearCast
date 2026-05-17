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
    public record SizeUpdateDto(Size SizeName, int QuantityChange, int? AvailableStock);

    public void AddOrUpdateSizes(IEnumerable<SizeUpdateDto> sizeUpdates)
    {
        foreach (var update in sizeUpdates)
        {
            var existingSize = Sizes.FirstOrDefault(s => s.Size == update.SizeName);
            int currentQuantity = existingSize?.Quantity ?? 0;
            int requestedQuantity = currentQuantity + update.QuantityChange;

            if (FixedColorId.HasValue && update.AvailableStock.HasValue)
            {
                if (update.QuantityChange > 0 && requestedQuantity > update.AvailableStock.Value)
                {
                    string cleanSizeName = update.SizeName.ToString().TrimStart('_').Replace("_", " ");
                    if (update.AvailableStock.Value == 0)
                    {
                        throw new InvalidOperationException($"Size {cleanSizeName} is currently out of stock.");
                    }
                    else
                    {
                        throw new InvalidOperationException($"Only {update.AvailableStock.Value} item(s) available in stock for size {cleanSizeName}.");
                    }
                }
            }
        }

        foreach (var update in sizeUpdates)
        {
            var existingSize = Sizes.FirstOrDefault(s => s.Size == update.SizeName);
            int currentQuantity = existingSize?.Quantity ?? 0;
            int requestedQuantity = currentQuantity + update.QuantityChange;

            if (requestedQuantity < 0)
            {
                requestedQuantity = 0;
            }

            if (existingSize != null)
            {
                if (requestedQuantity <= 0)
                {
                    Sizes.Remove(existingSize);
                }
                else
                {
                    existingSize.Quantity = requestedQuantity;
                }
            }
            else if (requestedQuantity > 0)
            {
                Sizes.Add(new FixedProductSize
                {
                    Size = update.SizeName,
                    Quantity = requestedQuantity
                });
            }
        }

        if (sizeUpdates.Any())
        {
            var sortedSizes = Sizes.OrderBy(s => s.Size).ToList();
            Sizes.Clear();
            foreach (var size in sortedSizes)
            {
                Sizes.Add(size);
            }
        }
    }

    public void RemoveSize(Size size)
    {
        var item = Sizes.FirstOrDefault(s => s.Size == size);
        if (item != null) Sizes.Remove(item);
    }
}
