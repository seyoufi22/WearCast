namespace WearCast.Api.Common.Tracking.Models
{
    public class ProductDetails
    {
        public decimal Price { get; set; }
        public List<string> TargetAudience { get; set; } = new();
        public string? DressStyle { get; set; } = string.Empty;
        public string? CategoryName { get; set; } = string.Empty;
        public int? SellerId { get; set; }
    }
}
