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

    public Result AddOrUpdateSizes(IEnumerable<SizeUpdateDto> sizeUpdates)
    {
        // Loop 1: Validation and stock check
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

                    // Return Result.Failure instead of throwing an Exception to avoid breaking control flow
                    if (update.AvailableStock.Value == 0)
                    {
                        return Result.Failure(new Error("Cart.OutOfStock", $"Size {cleanSizeName} is currently out of stock.", 400));
                    }
                    else
                    {
                        return Result.Failure(new Error("Cart.StockExceeded", $"Only {update.AvailableStock.Value} item(s) available in stock for size {cleanSizeName}.", 400));
                    }
                }
            }
        }

        // Loop 2: Apply changes to the tracked instances
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
                    // Update the existing tracked instance directly
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

        return Result.Success();
    }

    public void RemoveSize(Size size)
    {
        var item = Sizes.FirstOrDefault(s => s.Size == size);
        if (item != null) Sizes.Remove(item);
    }
}
