namespace WearCast.Api.Entities.DesignedProducts
{
    public class DesignedProductColor : BaseModel, ISoftDeletable
    {
        public string Name { get; set; } = string.Empty;     // مثلاً: أسود غامق
        public string HexCode { get; set; } = string.Empty;  // مثلاً: #000000
        public string MainImageUrl { get; set; } = string.Empty;


        public int DesignedProductId { get; set; }
        public DesignedProduct DesignedProduct { get; set; } = default!;

        public ICollection<DesignedProductImage> Images { get; set; } = [];
    }
}
