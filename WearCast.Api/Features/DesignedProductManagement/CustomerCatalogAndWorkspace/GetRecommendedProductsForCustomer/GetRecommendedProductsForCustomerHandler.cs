using System.Text.Json;
using Microsoft.AspNetCore.Http;
using WearCast.Api.Common.ExternalServices;
using WearCast.Api.Common.Helper;

namespace WearCast.Api.Features.DesignedProductManagement.CustomerCatalogAndWorkspace.GetRecommendedProductsForCustomer
{
    public class GetRecommendedProductsForCustomerHandler(
        ApplicationDbContext context,
        IRecommendationServiceClient recommendationClient,
        IHttpContextAccessor httpContextAccessor
        ) : IRequestHandler<GetRecommendedProductsForCustomerRequest, Result<List<GetRecommendedProductsForCustomerResponse>>>
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IRecommendationServiceClient _recommendationClient = recommendationClient;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        private static readonly JsonSerializerOptions _jsonOpts = new() { PropertyNameCaseInsensitive = true };

        // ═══════════════════════════════════════════════════════════════
        // ENTRY POINT
        // ═══════════════════════════════════════════════════════════════
        public async Task<Result<List<GetRecommendedProductsForCustomerResponse>>> Handle(
            GetRecommendedProductsForCustomerRequest request,
            CancellationToken cancellationToken)
        {
            // ── AUTH CHECK ──────────────────────────────────────────────
            var user = _httpContextAccessor.HttpContext?.User;
            if (user == null || !user.IsCustomer())
                return Result.Failure<List<GetRecommendedProductsForCustomerResponse>>(
                    new Error("User.NotFound", "User not found or not a customer", StatusCodes.Status404NotFound));

            var userId = user.GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Result.Failure<List<GetRecommendedProductsForCustomerResponse>>(
                    new Error("User.IdNotFound", "User ID not found", StatusCodes.Status404NotFound));

            Console.WriteLine($"");
            Console.WriteLine($"╔══════════════════════════════════════════════════════════════╗");
            Console.WriteLine($"║  [RECOMMENDATION ENGINE] START  |  User: {userId,-22} ║");
            Console.WriteLine($"║  TopK requested: {request.TopK,-43} ║");
            Console.WriteLine($"╚══════════════════════════════════════════════════════════════╝");

            // ── STEP 1: ACTIVITY LOGS → ML FILTERS ─────────────────────
            Console.WriteLine($"[STEP 1] Reading user activity logs from DB...");
            var (mlFilters, logStats) = await BuildFiltersFromActivityLogsAsync(userId, cancellationToken);

            Console.WriteLine($"[STEP 1] Activity log summary:");
            Console.WriteLine($"         Total logs fetched : {logStats.TotalLogs}");
            Console.WriteLine($"         Filter events      : {logStats.FilterEvents}");
            Console.WriteLine($"         Click/View events  : {logStats.ClickViewEvents}");
            Console.WriteLine($"         Purchase events    : {logStats.PurchaseEvents}");
            Console.WriteLine($"         Malformed entries  : {logStats.MalformedEntries}");

            if (mlFilters != null)
            {
                Console.WriteLine($"[STEP 1] ✅ ML filters derived from activity:");
                Console.WriteLine($"         CategoryName   = {mlFilters.CategoryName ?? "(none)"}");
                Console.WriteLine($"         DressStyle     = {mlFilters.DressStyle ?? "(none)"}");
                Console.WriteLine($"         TargetAudience = {(mlFilters.TargetAudience != null ? string.Join(", ", mlFilters.TargetAudience) : "(none)")}");
                Console.WriteLine($"         MinPrice       = {mlFilters.MinPrice?.ToString() ?? "(none)"}");
                Console.WriteLine($"         MaxPrice       = {mlFilters.MaxPrice?.ToString() ?? "(none)"}");
            }
            else
            {
                Console.WriteLine($"[STEP 1] ⚠️  No filters derived — user has no activity logs or logs yielded no signals.");
                Console.WriteLine($"         The ML service will use PURE collaborative/content-based recommendations.");
            }

            // ── STEP 2: CALL ML SERVICE ─────────────────────────────────
            Console.WriteLine($"[STEP 2] Calling ML recommendation service...");
            var recommendedProductIds = await _recommendationClient.GetRecommendationsAsync(
                userId, request.TopK, mlFilters, excludeSeen: false);

            Console.WriteLine($"[STEP 2] ML service returned {recommendedProductIds.Count} product ID(s).");

            if (recommendedProductIds.Count == 0)
            {
                Console.WriteLine($"[STEP 2] ❌ ML returned 0 results → FALLBACK activated.");
                Console.WriteLine($"         Possible reasons:");
                Console.WriteLine($"         • ML service is still initializing (HTTP 503)");
                Console.WriteLine($"         • User {userId} is not in the ML training data");
                Console.WriteLine($"         • All ML candidates were filtered out");
                Console.WriteLine($"         • Network/DNS error reaching the ML service");
                Console.WriteLine($"[STEP 2] Using newest-products fallback (50/50 split, not ML ranked).");
                var fallback = await BuildSmartFallbackAsync(request.TopK, cancellationToken);
                Console.WriteLine($"[STEP 2] Fallback returned {fallback.Count} products.");
                Console.WriteLine($"─────────────────── END (FALLBACK) ───────────────────");
                return Result.Success(fallback);
            }

            Console.WriteLine($"[STEP 2] ✅ ML success. All {recommendedProductIds.Count} raw IDs:");
            for (int i = 0; i < recommendedProductIds.Count; i++)
                Console.WriteLine($"         [{i + 1:D2}] {recommendedProductIds[i]}");

            // ── STEP 3: PARSE FIX_/DES_ PREFIXES ───────────────────────
            Console.WriteLine($"[STEP 3] Parsing product ID prefixes...");
            var forFixed    = new List<int>();
            var forDesigned = new List<int>();
            var unparseable = new List<string>();

            foreach (var fullId in recommendedProductIds)
            {
                if (fullId.StartsWith("FIX_") && int.TryParse(fullId[4..], out int fid))
                    forFixed.Add(fid);
                else if (fullId.StartsWith("DES_") && int.TryParse(fullId[4..], out int did))
                    forDesigned.Add(did);
                else if (int.TryParse(fullId, out int id))
                    forDesigned.Add(id);
                else
                    unparseable.Add(fullId);
            }

            Console.WriteLine($"[STEP 3] Prefix parse result:");
            Console.WriteLine($"         FIX_ (custom product)   : {forFixed.Count} IDs → [{string.Join(", ", forFixed)}]");
            Console.WriteLine($"         DES_ (designed product) : {forDesigned.Count} IDs → [{string.Join(", ", forDesigned)}]");
            if (unparseable.Count > 0)
                Console.WriteLine($"         ⚠️  Unparseable IDs       : {unparseable.Count} → [{string.Join(", ", unparseable)}]");

            // ── STEP 4: HYDRATE FROM DB ─────────────────────────────────
            Console.WriteLine($"[STEP 4] Fetching product metadata from database...");

            var designedProducts = forDesigned.Count > 0
                ? await _context.DesignedProducts
                    .AsNoTracking()
                    .Where(p => forDesigned.Contains(p.Id))
                    .Include(p => p.Category)
                    .Select(p => new
                    {
                        p.Id,
                        p.Name,
                        p.Price,
                        CategoryName = p.Category.Name,
                        Image        = p.DefaultColor != null
                            ? p.DefaultColor.MainImageUrl
                            : p.Colors.Select(c => c.MainImageUrl).FirstOrDefault()
                    })
                    .ToListAsync(cancellationToken)
                : [];

            var fixedProducts = forFixed.Count > 0
                ? await _context.FixedProducts
                    .AsNoTracking()
                    .Where(p => forFixed.Contains(p.Id))
                    .Include(p => p.Category)
                    .Select(p => new
                    {
                        p.Id,
                        p.Name,
                        p.Price,
                        CategoryName = p.Category.Name,
                        Image        = p.Colors.Select(c => c.ImageUrl).FirstOrDefault()
                    })
                    .ToListAsync(cancellationToken)
                : [];

            Console.WriteLine($"[STEP 4] DB hydration result:");
            Console.WriteLine($"         DesignedProducts found in DB : {designedProducts.Count} / {forDesigned.Count} requested");
            Console.WriteLine($"         FixedProducts found in DB    : {fixedProducts.Count} / {forFixed.Count} requested");

            var missingDesigned = forDesigned.Except(designedProducts.Select(p => p.Id)).ToList();
            var missingFixed    = forFixed.Except(fixedProducts.Select(p => p.Id)).ToList();
            if (missingDesigned.Count > 0)
                Console.WriteLine($"         ⚠️  Missing DesignedProduct IDs : [{string.Join(", ", missingDesigned)}] — ML catalog may be out of sync with DB");
            if (missingFixed.Count > 0)
                Console.WriteLine($"         ⚠️  Missing FixedProduct IDs    : [{string.Join(", ", missingFixed)}] — ML catalog may be out of sync with DB");

            // ── STEP 5: RECONSTRUCT IN ML RANK ORDER ───────────────────
            Console.WriteLine($"[STEP 5] Reconstructing response in ML rank order...");
            var finalResponses = new List<GetRecommendedProductsForCustomerResponse>(recommendedProductIds.Count);

            foreach (var fullId in recommendedProductIds)
            {
                if (fullId.StartsWith("FIX_") && int.TryParse(fullId[4..], out int fid))
                {
                    var p = fixedProducts.FirstOrDefault(x => x.Id == fid);
                    if (p != null)
                        finalResponses.Add(new GetRecommendedProductsForCustomerResponse
                        {
                            Id           = p.Id,
                            Name         = p.Name,
                            Price        = p.Price,
                            CategoryName = p.CategoryName,
                            MainImageUrl = p.Image,
                            Type         = "custom product"
                        });
                    else
                        Console.WriteLine($"[STEP 5] ⚠️  Skipped {fullId} — not found in DB hydration result");
                }
                else
                {
                    string cleanId = fullId.StartsWith("DES_") ? fullId[4..] : fullId;
                    if (int.TryParse(cleanId, out int did))
                    {
                        var p = designedProducts.FirstOrDefault(x => x.Id == did);
                        if (p != null)
                            finalResponses.Add(new GetRecommendedProductsForCustomerResponse
                            {
                                Id           = p.Id,
                                Name         = p.Name,
                                Price        = p.Price,
                                CategoryName = p.CategoryName,
                                MainImageUrl = p.Image,
                                Type         = "designed product"
                            });
                        else
                            Console.WriteLine($"[STEP 5] ⚠️  Skipped {fullId} — not found in DB hydration result");
                    }
                }
            }

            // ── STEP 5.5: PAD WITH FALLBACK IF SHORT ────────────────────
            if (finalResponses.Count < request.TopK)
            {
                int needed = request.TopK - finalResponses.Count;
                Console.WriteLine($"[STEP 5.5] Padding with {needed} fallback items to reach topK={request.TopK}...");
                
                // Fetch enough fallback items to guarantee we can fill the gap even if there are overlaps
                var fallbackItems = await BuildSmartFallbackAsync(request.TopK + finalResponses.Count, cancellationToken);
                
                var existingDesignedIds = finalResponses.Where(x => x.Type == "designed product").Select(x => x.Id).ToHashSet();
                var existingFixedIds = finalResponses.Where(x => x.Type == "custom product").Select(x => x.Id).ToHashSet();
                
                foreach (var item in fallbackItems)
                {
                    if (finalResponses.Count >= request.TopK) break;
                    
                    bool isDuplicate = (item.Type == "designed product" && existingDesignedIds.Contains(item.Id)) ||
                                       (item.Type == "custom product" && existingFixedIds.Contains(item.Id));
                                       
                    if (!isDuplicate)
                    {
                        finalResponses.Add(item);
                        if (item.Type == "designed product") existingDesignedIds.Add(item.Id);
                        else existingFixedIds.Add(item.Id);
                    }
                }
            }

            // ── STEP 6: FINAL SUMMARY ───────────────────────────────────
            var customCount   = finalResponses.Count(r => r.Type == "custom product");
            var designedCount = finalResponses.Count(r => r.Type == "designed product");

            Console.WriteLine($"[STEP 6] Final response summary:");
            Console.WriteLine($"         Total items returned    : {finalResponses.Count}");
            Console.WriteLine($"         → custom product        : {customCount}");
            Console.WriteLine($"         → designed product      : {designedCount}");
            Console.WriteLine($"         Source                  : ML (AI ranked) + Fallback padding");

            if (finalResponses.Count < request.TopK)
                Console.WriteLine($"         ⚠️  Returned fewer than requested ({finalResponses.Count} < {request.TopK}) — not enough products in the catalog.");

            Console.WriteLine($"╚══════════════════ END (ML SUCCESS) ══════════════════╝");
            Console.WriteLine($"");

            return Result.Success(finalResponses);
        }

        // ═══════════════════════════════════════════════════════════════
        // BUILD ML FILTERS FROM ACTIVITY LOGS
        // ═══════════════════════════════════════════════════════════════
        private async Task<(RecommendationFilterContext? filters, ActivityLogStats stats)>
            BuildFiltersFromActivityLogsAsync(string userId, CancellationToken ct)
        {
            var stats = new ActivityLogStats();

            var rawLogs = await _context.UserActivityLogs
                .AsNoTracking()
                .Where(l => l.UserId == userId)
                .OrderByDescending(l => l.CreatedAt)
                .Take(50)
                .Select(l => l.Payload)
                .ToListAsync(ct);

            stats.TotalLogs = rawLogs.Count;

            if (rawLogs.Count == 0)
                return (null, stats);

            var categories  = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            var dressStyles = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            var audiences   = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            var pricePoints = new List<decimal>();

            foreach (var payload in rawLogs)
            {
                try
                {
                    using var doc  = JsonDocument.Parse(payload);
                    var root       = doc.RootElement;
                    string evtType = "";
                    if (root.TryGetProperty("eventType", out var et))
                        evtType = et.GetString()?.ToLower() ?? "";

                    if (evtType == "filter" && root.TryGetProperty("filters", out var f))
                    {
                        stats.FilterEvents++;
                        Tally(f, "categoryName", categories, weight: 3);
                        Tally(f, "dressStyle",   dressStyles, weight: 3);
                        TallyAudience(f, audiences, weight: 3);
                        TallyPrice(f, pricePoints);
                    }

                    if ((evtType == "click" || evtType == "view") &&
                        root.TryGetProperty("productDetails", out var pd))
                    {
                        stats.ClickViewEvents++;
                        Tally(pd, "categoryName", categories,  weight: 1);
                        Tally(pd, "dressStyle",   dressStyles, weight: 1);
                        TallyAudience(pd, audiences, weight: 1);
                        if (pd.TryGetProperty("price", out var priceEl) && priceEl.TryGetDecimal(out var price))
                            pricePoints.Add(price);
                    }

                    if (evtType == "purchase" && root.TryGetProperty("products", out var products))
                    {
                        stats.PurchaseEvents++;
                        foreach (var item in products.EnumerateArray())
                        {
                            if (item.TryGetProperty("productDetails", out var ipd))
                            {
                                Tally(ipd, "categoryName", categories,  weight: 2);
                                Tally(ipd, "dressStyle",   dressStyles, weight: 2);
                                TallyAudience(ipd, audiences, weight: 2);
                            }
                        }
                    }
                }
                catch
                {
                    stats.MalformedEntries++;
                }
            }

            if (categories.Count == 0 && dressStyles.Count == 0 && audiences.Count == 0)
                return (null, stats);

            decimal? minPrice = null, maxPrice = null;
            if (pricePoints.Count >= 3)
            {
                pricePoints.Sort();
                int lo = (int)(pricePoints.Count * 0.10);
                int hi = (int)(pricePoints.Count * 0.90);
                minPrice = pricePoints[lo];
                maxPrice = pricePoints[Math.Min(hi, pricePoints.Count - 1)];
            }

            return (new RecommendationFilterContext
            {
                CategoryName   = TopKey(categories),
                DressStyle     = TopKey(dressStyles),
                TargetAudience = audiences.Count > 0 ? TopAudiences(audiences) : null,
                MinPrice       = minPrice,
                MaxPrice       = maxPrice
            }, stats);
        }

        // ═══════════════════════════════════════════════════════════════
        // FALLBACK (ML unavailable / returned 0 results)
        // ═══════════════════════════════════════════════════════════════
        private async Task<List<GetRecommendedProductsForCustomerResponse>> BuildSmartFallbackAsync(
            int topK, CancellationToken ct)
        {
            int designedCount = topK / 2;
            int customCount   = topK - designedCount;

            var newestDesigned = await _context.DesignedProducts
                .AsNoTracking()
                .OrderByDescending(p => p.CreatedOn)
                .Take(designedCount)
                .Select(p => new GetRecommendedProductsForCustomerResponse
                {
                    Id           = p.Id,
                    Name         = p.Name,
                    Price        = p.Price,
                    CategoryName = p.Category.Name,
                    MainImageUrl = p.DefaultColor != null
                        ? p.DefaultColor.MainImageUrl
                        : p.Colors.Select(c => c.MainImageUrl).FirstOrDefault(),
                    Type = "designed product"
                })
                .ToListAsync(ct);

            var newestFixed = await _context.FixedProducts
                .AsNoTracking()
                .OrderByDescending(p => p.CreatedOn)
                .Take(customCount)
                .Select(p => new GetRecommendedProductsForCustomerResponse
                {
                    Id           = p.Id,
                    Name         = p.Name,
                    Price        = p.Price,
                    CategoryName = p.Category.Name,
                    MainImageUrl = p.Colors.Select(c => c.ImageUrl).FirstOrDefault(),
                    Type = "custom product"
                })
                .ToListAsync(ct);

            var result = new List<GetRecommendedProductsForCustomerResponse>(topK);
            int max = Math.Max(newestDesigned.Count, newestFixed.Count);
            for (int i = 0; i < max; i++)
            {
                if (i < newestDesigned.Count) result.Add(newestDesigned[i]);
                if (i < newestFixed.Count)    result.Add(newestFixed[i]);
            }
            return result.Take(topK).ToList();
        }

        // ═══════════════════════════════════════════════════════════════
        // HELPERS
        // ═══════════════════════════════════════════════════════════════
        private static void Tally(JsonElement element, string propertyName,
            Dictionary<string, int> bag, int weight)
        {
            if (element.TryGetProperty(propertyName, out var prop))
            {
                var val = prop.ValueKind == JsonValueKind.String ? prop.GetString() : null;
                if (!string.IsNullOrWhiteSpace(val))
                    bag[val] = bag.GetValueOrDefault(val, 0) + weight;
            }
        }

        private static void TallyAudience(JsonElement element,
            Dictionary<string, int> bag, int weight)
        {
            if (!element.TryGetProperty("targetAudience", out var prop)) return;

            if (prop.ValueKind == JsonValueKind.Array)
            {
                foreach (var a in prop.EnumerateArray())
                {
                    var s = a.ValueKind == JsonValueKind.String ? a.GetString() : null;
                    if (!string.IsNullOrWhiteSpace(s))
                        bag[s] = bag.GetValueOrDefault(s, 0) + weight;
                }
            }
            else if (prop.ValueKind == JsonValueKind.String)
            {
                var s = prop.GetString();
                if (!string.IsNullOrWhiteSpace(s))
                    bag[s] = bag.GetValueOrDefault(s, 0) + weight;
            }
        }

        private static void TallyPrice(JsonElement element, List<decimal> pricePoints)
        {
            if (element.TryGetProperty("minPrice", out var lo) && lo.TryGetDecimal(out var minP))
                pricePoints.Add(minP);
            if (element.TryGetProperty("maxPrice", out var hi) && hi.TryGetDecimal(out var maxP))
                pricePoints.Add(maxP);
        }

        private static string? TopKey(Dictionary<string, int> bag)
            => bag.Count == 0 ? null : bag.MaxBy(kv => kv.Value).Key;

        private static List<string> TopAudiences(Dictionary<string, int> bag)
        {
            if (bag.Count == 0) return [];
            int max = bag.Values.Max();
            return bag.Where(kv => kv.Value >= max * 0.6)
                      .Select(kv => kv.Key)
                      .ToList();
        }

        // ─── stats holder ──────────────────────────────────────────────
        private record ActivityLogStats
        {
            public int TotalLogs       { get; set; }
            public int FilterEvents    { get; set; }
            public int ClickViewEvents { get; set; }
            public int PurchaseEvents  { get; set; }
            public int MalformedEntries { get; set; }
        }
    }
}
