namespace WearCast.Api.Common.ExternalServices
{
    /// <summary>
    /// Filters derived from the user's activity logs that are forwarded to the ML recommendation engine.
    /// Mirrors the FilterContext schema expected by the Python /recommend endpoint.
    /// </summary>
    public class RecommendationFilterContext
    {
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public List<string>? TargetAudience { get; set; }
        public string? DressStyle { get; set; }
        public string? CategoryName { get; set; }
        public string? SellerId { get; set; }
    }

    public interface IRecommendationServiceClient
    {
        Task<List<string>> GetRecommendationsAsync(
            string userId,
            int topK = 10,
            RecommendationFilterContext? filters = null,
            bool excludeSeen = false);
    }
}
