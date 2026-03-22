namespace WearCast.Api.Entities.DesignedProducts
{
    public class DesignedProductSizeDetails : ISoftDeletable
    {
        public int Id { get; set; }
        public Size Size { get; set; }
        public decimal? A { get; set; }
        public decimal? B { get; set; }
        public decimal? C { get; set; }

        public bool IsDeleted { get; set; }

        public int DesignedProductId { get; set; }
        public DesignedProduct DesignedProduct { get; set; } = default!;
    }
}
