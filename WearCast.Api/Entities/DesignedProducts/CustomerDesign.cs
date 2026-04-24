namespace WearCast.Api.Entities.DesignedProducts
{
    public class CustomerDesign : BaseModel, ISoftDeletable
    {
        public string Name { get; set; } = string.Empty;
        public string ViewDesignsJson { get; set; } = string.Empty;

        public string? FrontImageUrl { get; set; }
        public string? BackImageUrl { get; set; }
        public string? RightImageUrl { get; set; }
        public string? LeftImageUrl { get; set; }

        public int AssetCount { get; set; }

        public decimal TotalPrice { get; set; }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; } = default!;

        public int DesignedProductId { get; set; }
        public DesignedProduct DesignedProduct { get; set; } = default!;

        public int DesignedProductColorId { get; set; }
        public DesignedProductColor DesignedProductColor { get; set; } = default!;

        public int? CartItemId { get; set; }
        public CartItem? CartItem { get; set; }

        public void CalculateAndSetTotalPrice(decimal templatePrice, decimal fixedAssetPrice)
        {

            TotalPrice = templatePrice + (AssetCount * fixedAssetPrice);

        }
    }
}
