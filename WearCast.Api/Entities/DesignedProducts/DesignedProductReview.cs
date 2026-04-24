namespace WearCast.Api.Entities.DesignedProducts
{
    public class DesignedProductReview : BaseModel, ISoftDeletable
    {
        public int Rating { get; set; }
        public string? Comment { get; set; }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; } = default!;

        public int DesignedProductId { get; set; }
        public DesignedProduct DesignedProduct { get; set; } = default!;
    }
}
