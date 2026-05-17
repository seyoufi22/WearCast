namespace WearCast.Api.Entities.FixedProduct;

public class FixedProductReview : BaseModel, ISoftDeletable
{
    public int Rating { get; set; } 
    public string? Comment { get; set; }

    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = default!;

    public int FixedProductId { get; set; }
    public FixedProduct FixedProduct { get; set; } = default!;
}