namespace WearCast.Api.Common.Tracking.Models
{
    public class PurchaseProductDetails : ProductDetails
    {
        public string ProductId { get; set; } = string.Empty;
        public int Quantity { get; set; }
    }
}
