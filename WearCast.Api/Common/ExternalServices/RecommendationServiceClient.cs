using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace WearCast.Api.Common.ExternalServices
{
    public class RecommendationServiceClient(HttpClient httpClient, IConfiguration configuration) : IRecommendationServiceClient
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly string _baseUrl = configuration["RecommendationServiceSettings:BaseUrl"]
            ?? "https://wear-cast-recommendation-system-1.vercel.app";

        // Retry config: 3 attempts, wait 4s → 8s → 12s between retries (linear backoff)
        private const int    MaxRetries     = 3;
        private const int    RetryBaseDelay = 4_000; // ms

        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNamingPolicy        = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition      = JsonIgnoreCondition.WhenWritingNull,
            PropertyNameCaseInsensitive = true
        };

        public async Task<List<string>> GetRecommendationsAsync(
            string userId,
            int    topK        = 10,
            RecommendationFilterContext? filters = null,
            bool   excludeSeen = false)
        {
            var tag = $"[ML-Client][{userId[..Math.Min(8, userId.Length)]}...]";

            Console.WriteLine($"");
            Console.WriteLine($"{tag} ══════════════════════════════════════════════");
            Console.WriteLine($"{tag} POST {_baseUrl}/recommend");
            Console.WriteLine($"{tag} topK={topK} | excludeSeen={excludeSeen} | hasFilters={filters != null}");

            // ── Build request payload ─────────────────────────────────────────
            var requestBody = BuildRequestBody(userId, topK, excludeSeen, filters);
            var json        = JsonSerializer.Serialize(requestBody, _jsonOptions);
            Console.WriteLine($"{tag} Payload → {json}");

            // ── Retry loop ────────────────────────────────────────────────────
            for (int attempt = 1; attempt <= MaxRetries; attempt++)
            {
                Console.WriteLine($"{tag} Attempt {attempt}/{MaxRetries}...");

                var (productIds, shouldRetry, rawStatus, rawBody) =
                    await TrySendAsync(tag, json, attempt);

                if (!shouldRetry)
                {
                    Console.WriteLine($"{tag} ══════════════════════════════════════════════");
                    Console.WriteLine($"");
                    return productIds;
                }

                // Only retry on 503 "initializing" — the service is cold-starting
                if (attempt < MaxRetries)
                {
                    int delay = RetryBaseDelay * attempt;
                    Console.WriteLine($"{tag} ⏳ HTTP {rawStatus} — service still initializing.");
                    Console.WriteLine($"{tag}    Waiting {delay / 1000}s before retry {attempt + 1}/{MaxRetries}...");
                    await Task.Delay(delay);
                }
                else
                {
                    Console.WriteLine($"{tag} ❌ All {MaxRetries} attempts failed.");
                    Console.WriteLine($"{tag}    Last HTTP status : {rawStatus}");
                    Console.WriteLine($"{tag}    Last response    : {rawBody}");
                    Console.WriteLine($"{tag} ══════════════════════════════════════════════");
                    Console.WriteLine($"");
                    return [];
                }
            }

            return [];
        }

        // ── Single attempt ────────────────────────────────────────────────────
        private async Task<(List<string> ids, bool shouldRetry, int statusCode, string rawBody)>
            TrySendAsync(string tag, string json, int attempt)
        {
            try
            {
                var content  = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{_baseUrl}/recommend", content);

                int    code    = (int)response.StatusCode;
                string rawBody = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"{tag} [Attempt {attempt}] Status: {code}");
                Console.WriteLine($"{tag} [Attempt {attempt}] Body  : {rawBody[..Math.Min(400, rawBody.Length)]}");

                // ── 2xx — success ─────────────────────────────────────────────
                if (response.IsSuccessStatusCode)
                {
                    var ids = ParseProductIds(tag, attempt, rawBody);
                    return (ids, false, code, rawBody);
                }

                // ── 503 "initializing" — retry ────────────────────────────────
                if (code == 503 && rawBody.Contains("initializing", StringComparison.OrdinalIgnoreCase))
                    return ([], true, code, rawBody);

                // ── Any other error — no point retrying ───────────────────────
                Console.WriteLine($"{tag} [Attempt {attempt}] ❌ Non-retryable error ({code}) — giving up.");
                return ([], false, code, rawBody);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"{tag} [Attempt {attempt}] ❌ Network error: {ex.Message}");
                return ([], false, 0, ex.Message);
            }
            catch (TaskCanceledException ex)
            {
                Console.WriteLine($"{tag} [Attempt {attempt}] ❌ Timeout: {ex.Message}");
                return ([], false, 0, "timeout");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{tag} [Attempt {attempt}] ❌ Unexpected: {ex.GetType().Name} — {ex.Message}");
                return ([], false, 0, ex.Message);
            }
        }

        // ── Parse productId list from JSON response ───────────────────────────
        private List<string> ParseProductIds(string tag, int attempt, string rawBody)
        {
            try
            {
                var result = JsonSerializer.Deserialize<MlResponse>(rawBody, _jsonOptions);
                if (result == null)
                {
                    Console.WriteLine($"{tag} [Attempt {attempt}] ⚠️  Deserialized result is null.");
                    return [];
                }

                var ids = result.Recommendations
                    .Select(r => r.ProductId)
                    .Where(id => !string.IsNullOrWhiteSpace(id))
                    .ToList();

                Console.WriteLine($"{tag} [Attempt {attempt}] ✅ ML returned {ids.Count} valid product IDs.");
                if (ids.Count > 0)
                {
                    var preview = string.Join(", ", ids.Take(5));
                    Console.WriteLine($"{tag} [Attempt {attempt}] Preview → [{preview}{(ids.Count > 5 ? ", ..." : "")}]");

                    // type breakdown
                    int fixCount = ids.Count(id => id.StartsWith("FIX_"));
                    int desCount = ids.Count(id => id.StartsWith("DES_"));
                    Console.WriteLine($"{tag} [Attempt {attempt}] FIX_ (custom)  : {fixCount}");
                    Console.WriteLine($"{tag} [Attempt {attempt}] DES_ (designed) : {desCount}");
                }
                else
                {
                    Console.WriteLine($"{tag} [Attempt {attempt}] ⚠️  0 valid IDs in ML response.");
                    Console.WriteLine($"{tag}    Full recommendations array: {JsonSerializer.Serialize(result.Recommendations)}");
                    Console.WriteLine($"{tag}    This usually means: user not in ML training data, or all candidates filtered out.");
                }

                return ids;
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"{tag} [Attempt {attempt}] ❌ JSON parse error: {ex.Message}");
                Console.WriteLine($"{tag}    Full raw body: {rawBody}");
                return [];
            }
        }

        // ── Build request object ──────────────────────────────────────────────
        private static object BuildRequestBody(
            string userId, int topK, bool excludeSeen,
            RecommendationFilterContext? filters)
        {
            if (filters == null)
                return new { userId, topK, excludeSeen };

            return new
            {
                userId,
                topK,
                excludeSeen,
                filters = new
                {
                    minPrice       = filters.MinPrice,
                    maxPrice       = filters.MaxPrice,
                    targetAudience = filters.TargetAudience,
                    dressStyle     = filters.DressStyle,
                    categoryName   = filters.CategoryName,
                    sellerId       = filters.SellerId
                }
            };
        }

        // ── Private response models ───────────────────────────────────────────
        private class MlResponse
        {
            public List<MlItem> Recommendations { get; set; } = [];
            public string? Model  { get; set; }
            public string? UserId { get; set; }
        }

        private class MlItem
        {
            public string  ProductId { get; set; } = string.Empty;
            public float?  Score     { get; set; }
            public string? Type      { get; set; }
        }
    }
}
