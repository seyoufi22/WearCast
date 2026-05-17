namespace WearCast.Api.Common.Tracking.Models
{
    public class FilterDetails
    {
        public string? SearchKey { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public List<string>? TargetAudience { get; set; }
        public string? DressStyle { get; set; }
        public string? CategoryName { get; set; }
        public int? SellerId { get; set; }
    }
}
