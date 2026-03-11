namespace WearCast.Api.Entities.DesignedProducts
{
    public class CustomerDesign : BaseModel
    {
        public string Name { get; set; } = string.Empty;

        public string ViewDesignsJson { get; set; } = string.Empty;

        public int CustomerId { get; set; }
        public Customer Customer { get; set; } = default!;

        public int DesignedProductId { get; set; }
        public DesignedProduct DesignedProduct { get; set; } = default!;

        public int DesignedProductColorId { get; set; }
        public DesignedProductColor DesignedProductColor { get; set; } = default!;

    }
}
