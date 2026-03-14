namespace WearCast.Api.Entities.DesignedProducts
{
    public class DesignedProductColor : BaseModel
    {
        public string Name { get; set; } = string.Empty;     // مثلاً: أسود غامق
        public string Slug { get; set; } = string.Empty;     // مثلاً: black
        public string HexCode { get; set; } = string.Empty;  // مثلاً: #000000


        public int DesignedProductId { get; set; }
        public DesignedProduct DesignedProduct { get; set; } = default!;

        public ICollection<DesignedProductImage> Images { get; set; } = [];
    }
}
