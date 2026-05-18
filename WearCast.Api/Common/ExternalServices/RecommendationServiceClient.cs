using System.Text;
using System.Text.Json;

namespace WearCast.Api.Common.ExternalServices
{
    public class RecommendationServiceClient(HttpClient httpClient, IConfiguration configuration) : IRecommendationServiceClient
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly string _baseUrl = configuration["RecommendationServiceSettings:BaseUrl"] ?? "https://wear-cast-recommendation-system-1.vercel.app";

        public async Task<List<string>> GetRecommendationsAsync(string userId, int topK = 10)
        {
            try
            {
                var requestBody = new
                {
                    userId = userId,
                    topK = topK,
                    excludeSeen = false
                };

                var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{_baseUrl}/recommend", content);

                if (!response.IsSuccessStatusCode)
                {
                    return [];
                }

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<RecommendationResponse>(jsonResponse, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

                return result?.Recommendations.Select(r => r.ProductId).ToList() ?? [];
            }
            catch
            {
                // Fallback to empty list if service is down
                return [];
            }
        }

        private class RecommendationResponse
        {
            public List<RecommendedItem> Recommendations { get; set; } = [];
        }

        private class RecommendedItem
        {
            public string ProductId { get; set; } = string.Empty;
        }
    }
}
